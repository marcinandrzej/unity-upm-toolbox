using MA.Serialization;
using UnityEngine;

namespace MA.Systems.Settings
{
    public abstract class SettingsManagerGeneric<SettingsDataT> : MonoBehaviour, ISettingsManagerGeneric<SettingsDataT> where SettingsDataT : new()
    {
        [SerializeField] private string settingsFileName = "Settings";
        [SerializeField] private SerializerBase serializer;

        private SettingsDataT _settingsData;

        public SettingsDataT Settings => _settingsData;

        public void SetDefaultSettings() => _settingsData = new SettingsDataT();

        public ESerializationOperationResult LoadSettings()
        {
            (SettingsDataT data, ESerializationOperationResult result) operation = serializer.DeserializeDataFromFile<SettingsDataT>(Application.persistentDataPath, settingsFileName);

            if (operation.result.HasFlag(ESerializationOperationResult.Success))
                _settingsData = operation.data;

            return operation.result;
        }

        public async Awaitable<ESerializationOperationResult> LoadSettingsAsync()
        {
            (SettingsDataT data, ESerializationOperationResult result) operation = await serializer.DeserializeDataFromFileAsync<SettingsDataT>(Application.persistentDataPath, settingsFileName);

            if (operation.result.HasFlag(ESerializationOperationResult.Success))
                _settingsData = operation.data;

            return operation.result;
        }

        public ESerializationOperationResult SaveSettings() => serializer.SerializeDataToFile<SettingsDataT>(Application.persistentDataPath, settingsFileName, _settingsData);

        public async Awaitable<ESerializationOperationResult> SaveSettingsAsync() => await serializer.SerializeDataToFileAsync<SettingsDataT>(Application.persistentDataPath, settingsFileName, _settingsData);
    }
}
