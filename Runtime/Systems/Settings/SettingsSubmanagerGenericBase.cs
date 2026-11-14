namespace MA.Systems.Settings
{
    public abstract class SettingsSubmanagerGenericBase<SettingsDataT>
    {
        public abstract bool IsDirty { get; }

        public abstract void InitializeData();

        public abstract void RevertToSavedData();

        public abstract void RevertToDefaultData();

        public abstract void Apply();

        public abstract void LoadFrom(SettingsDataT settingsData);

        public abstract void SaveTo(SettingsDataT settingsData);
    }
}
