using MA.Serialization;
using UnityEngine;
using UnityEngine.Audio;

namespace MA.Systems.Settings.Samples
{
    public class SampleSettingsManager : SettingsManagerGeneric<SampleSettingsData>
    {
        [SerializeField] private AudioMixer audioMixer;

        private void Awake()
        {
            Initialize();

            ESerializationOperationResult loadResult = LoadSettings();

            if (!loadResult.HasFlag(ESerializationOperationResult.Success))
                SaveSettings();

            Apply();
        }

        protected override void RegisterSubmanagers()
        {
            TryRegisterSubmanager(new SampleResolutionSubmanager<SampleSettingsData>());
            TryRegisterSubmanager(new SampleAudioSubmanager<SampleSettingsData>(audioMixer));
        }
    }
}
