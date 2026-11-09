using MA.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace MA.Systems.Save
{
    public interface ISaveManagerGeneric<HeaderDataT, GameDataT>
    {
        public (List<HeaderDataT> data, ESerializationOperationResult result) LoadHeaderDataList();

        public Awaitable<(List<HeaderDataT> data, ESerializationOperationResult result)> LoadHeaderDataListAsync();

        public (GameDataT data, ESerializationOperationResult result) LoadGameData(HeaderDataT headerData);

        public Awaitable<(GameDataT data, ESerializationOperationResult result)> LoadGameDataAsync(HeaderDataT headerData);

        public (HeaderDataT data, ESerializationOperationResult result)  SaveGameData(GameDataT gameData, string saveName);

        public Awaitable<(HeaderDataT data, ESerializationOperationResult result)> SaveGameDataAsync(GameDataT gameData, string saveName);

        public (ESerializationOperationResult headerResult, ESerializationOperationResult dataResult) DeleteSave(HeaderDataT header);
    }
}
