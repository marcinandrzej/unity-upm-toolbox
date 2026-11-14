using System.Collections.Generic;

namespace MA.Systems.Settings
{
    public abstract class SettingsSubmanagerGeneric<SettingsDataT, SettingsSubDataT> : SettingsSubmanagerGenericBase<SettingsDataT>
        where SettingsDataT : ISettingsDataGeneric<SettingsSubDataT>
        where SettingsSubDataT : struct
    {
        public SettingsSubDataT SavedData { get; private set; }

        public SettingsSubDataT CurrentData { get; set; }

        public abstract SettingsSubDataT DefaultData { get; }

        public override bool IsDirty => !EqualityComparer<SettingsSubDataT>.Default.Equals(SavedData, CurrentData);

        public override void InitializeData()
        {
            SavedData = DefaultData;
            CurrentData = DefaultData;
        }

        public override void RevertToSavedData() => CurrentData = SavedData;

        public override void RevertToDefaultData() => CurrentData = DefaultData;

        public override void LoadFrom(SettingsDataT settingsData)
        {
            SavedData = settingsData.GetData();
            CurrentData = SavedData;
        }

        public override void SaveTo(SettingsDataT settingsData) => settingsData.SetData(CurrentData);
    }
}

