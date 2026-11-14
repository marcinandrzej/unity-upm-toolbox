using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MA.Systems.Settings.Samples
{
    public class SampleResolutionSubmanager<SettingsDataT> : SettingsSubmanagerGeneric<SettingsDataT, SampleResolutionData> where SettingsDataT : ISettingsDataGeneric<SampleResolutionData>
    {
        private SampleResolutionData _defaultData;
        private List<SampleResolutionData> _validResolutions = new List<SampleResolutionData>();

        public IReadOnlyList<SampleResolutionData> ValidResolutions => _validResolutions;

        public override SampleResolutionData DefaultData => _defaultData;

        public override void InitializeData()
        {
            _defaultData = new(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.FullScreenWindow);

            _validResolutions = Screen.resolutions
            .Select(r => new SampleResolutionData(r.width, r.height, FullScreenMode.FullScreenWindow))
            .Distinct()
            .ToList();

            base.InitializeData();
        }

        public void SetScreenMode(FullScreenMode screenMode)
        {
            CurrentData = new SampleResolutionData(CurrentData.Width, CurrentData.Height, screenMode);
        }

        public void SetResolutionByIndex(int index)
        {
            if (index < 0 || index >= _validResolutions.Count)
                return;

            SampleResolutionData selected = _validResolutions[index];
            CurrentData = new SampleResolutionData(selected.Width, selected.Height, CurrentData.ScreenMode);
        }

        public override void Apply()
        {
            Screen.SetResolution(CurrentData.Width, CurrentData.Height, CurrentData.ScreenMode);
        }
    }
}
