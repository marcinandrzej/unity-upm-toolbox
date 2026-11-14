namespace MA.Systems.Settings
{
    public interface ISettingsDataGeneric<SettingsDataT> where SettingsDataT : struct
    {
        public SettingsDataT GetData();

        public void SetData(SettingsDataT data);
    }
}
