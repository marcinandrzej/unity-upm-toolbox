using MA.Serialization;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MA.Systems.Save
{
    public abstract class SaveManagerGeneric<HeaderDataT, GameDataT> : MonoBehaviour, ISaveManagerGeneric<HeaderDataT, GameDataT>
    {
        [SerializeField] protected string saveFolderName = "Saves";
        [SerializeField] protected string headerDataFolderName = "HeaderData";
        [SerializeField] protected string gameDataFolderName = "GameData";
        [SerializeField] protected SerializerBase serializer;

        protected string HeaderDataFolderPath => Path.Combine(Application.persistentDataPath, saveFolderName, headerDataFolderName);
        protected string GameDataFolderPath => Path.Combine(Application.persistentDataPath, saveFolderName, gameDataFolderName);

        public (List<HeaderDataT> data, ESerializationOperationResult result) LoadHeaderDataList() =>
            serializer.DeserializeDataFromDirectory<HeaderDataT>(HeaderDataFolderPath);

        public Awaitable<(List<HeaderDataT> data, ESerializationOperationResult result)> LoadHeaderDataListAsync() =>
            serializer.DeserializeDataFromDirectoryAsync<HeaderDataT>(HeaderDataFolderPath);

        public (GameDataT data, ESerializationOperationResult result) LoadGameData(HeaderDataT headerData)
        {
            string saveName = GetSaveFileName(headerData);

            return serializer.DeserializeDataFromFile<GameDataT>(GameDataFolderPath, saveName);
        }

        public Awaitable<(GameDataT data, ESerializationOperationResult result)> LoadGameDataAsync(HeaderDataT headerData)
        {
            string saveName = GetSaveFileName(headerData);

            return serializer.DeserializeDataFromFileAsync<GameDataT>(GameDataFolderPath, saveName);
        }

        public (HeaderDataT data, ESerializationOperationResult result) SaveGameData(GameDataT gameData, string saveName)
        {
            HeaderDataT header = GenerateHeaderData(saveName);

            ESerializationOperationResult headerResult = serializer.SerializeDataToFile<HeaderDataT>(HeaderDataFolderPath, saveName, header);

            if (headerResult.HasFlag(ESerializationOperationResult.Failure))
                return (default, headerResult);

            ESerializationOperationResult dataResult = serializer.SerializeDataToFile<GameDataT>(GameDataFolderPath, saveName, gameData);

            return (header, dataResult);
        }

        public async Awaitable<(HeaderDataT data, ESerializationOperationResult result)> SaveGameDataAsync(GameDataT gameData, string saveName)
        {
            HeaderDataT header = GenerateHeaderData(saveName);

            ESerializationOperationResult headerResult = await serializer.SerializeDataToFileAsync<HeaderDataT>(HeaderDataFolderPath, saveName, header);

            if (headerResult.HasFlag(ESerializationOperationResult.Failure))
                return (default, headerResult);

            ESerializationOperationResult dataResult = await serializer.SerializeDataToFileAsync<GameDataT>(GameDataFolderPath, saveName, gameData);

            return (header, dataResult);
        }

        public (ESerializationOperationResult headerResult, ESerializationOperationResult dataResult) DeleteSave(HeaderDataT header)
        {
            string saveName = GetSaveFileName(header);

            ESerializationOperationResult headerResult = serializer.DeleteFile(HeaderDataFolderPath, saveName);
            ESerializationOperationResult dataResult = serializer.DeleteFile(GameDataFolderPath, saveName);

            return (headerResult, dataResult);
        }

        protected abstract string GetSaveFileName(HeaderDataT headerData);

        protected abstract HeaderDataT GenerateHeaderData(string saveName);
    }
}
