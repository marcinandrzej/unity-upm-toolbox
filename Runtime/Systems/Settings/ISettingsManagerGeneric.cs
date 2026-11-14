using MA.Serialization;
using UnityEngine;

namespace MA.Systems.Settings
{
    public interface ISettingsManagerGeneric<SettingsDataT>
    {
        public void Initialize();

        public void Apply();

        public bool TryGetSubmanager<SubmanagerT>(out SubmanagerT submanager) where SubmanagerT : SettingsSubmanagerGenericBase<SettingsDataT>;

        public ESerializationOperationResult LoadSettings();

        public Awaitable<ESerializationOperationResult> LoadSettingsAsync();

        public ESerializationOperationResult SaveSettings();

        public Awaitable<ESerializationOperationResult> SaveSettingsAsync();
    }
}