using System;

namespace MA.Systems.Settings.Samples
{
    [Serializable]
    public class SampleSettingsData : ISettingsDataGeneric<SampleResolutionData>, ISettingsDataGeneric<SampleAudioData>
    {
        public SampleResolutionData resolutionData;
        public SampleAudioData audioData;

        SampleResolutionData ISettingsDataGeneric<SampleResolutionData>.GetData() => resolutionData;

        void ISettingsDataGeneric<SampleResolutionData>.SetData(SampleResolutionData data) => resolutionData = data;

        SampleAudioData ISettingsDataGeneric<SampleAudioData>.GetData() => audioData;

        void ISettingsDataGeneric<SampleAudioData>.SetData(SampleAudioData data) => audioData = data;
    }
}
