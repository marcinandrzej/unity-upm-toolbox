using MA.Serialization;
using UnityEngine;

namespace MA.Systems.Settings
{
    public interface ISettingsManagerGeneric<SettingsDataT>
    {
        public SettingsDataT Settings { get; }

        public void SetDefaultSettings();

        public ESerializationOperationResult LoadSettings();

        public Awaitable<ESerializationOperationResult> LoadSettingsAsync();

        public ESerializationOperationResult SaveSettings();

        public Awaitable<ESerializationOperationResult> SaveSettingsAsync();
    }
}