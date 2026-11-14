using UnityEngine;
using UnityEngine.Audio;

namespace MA.Systems.Settings.Samples
{
    public class SampleAudioSubmanager<SettingsDataT> : SettingsSubmanagerGeneric<SettingsDataT, SampleAudioData>
        where SettingsDataT : ISettingsDataGeneric<SampleAudioData>
    {
        private readonly AudioMixer _mixer;

        public override SampleAudioData DefaultData => new(1f, 0.8f, 1f, false);

        public SampleAudioSubmanager(AudioMixer mixer)
        {
            _mixer = mixer;
        }

        public void SetMasterVolume(float volume)
        {
            SampleAudioData data = CurrentData;
            data.MasterVolume = Mathf.Clamp01(volume);
            CurrentData = data;
        }

        public void SetMusicVolume(float volume)
        {
            SampleAudioData data = CurrentData;
            data.MusicVolume = Mathf.Clamp01(volume);
            CurrentData = data;
        }

        public void SetSfxVolume(float volume)
        {
            SampleAudioData data = CurrentData;
            data.SfxVolume = Mathf.Clamp01(volume);
            CurrentData = data;
        }

        public void SetMuted(bool isMuted)
        {
            SampleAudioData data = CurrentData;
            data.IsMuted = isMuted;
            CurrentData = data;
        }

        public override void Apply()
        {
            if (_mixer == null) 
                return;

            float effectiveMaster = CurrentData.IsMuted ? 0f : CurrentData.MasterVolume;
            _mixer.SetFloat("MasterVolume", LinearToDecibels(effectiveMaster));
            _mixer.SetFloat("MusicVolume", LinearToDecibels(CurrentData.MusicVolume));
            _mixer.SetFloat("SfxVolume", LinearToDecibels(CurrentData.SfxVolume));
        }

        private static float LinearToDecibels(float linear) => linear > 0.0001f ? Mathf.Log10(linear) * 20f : -80f;
    }
}
