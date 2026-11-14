using System;

namespace MA.Systems.Settings.Samples
{
    [Serializable]
    public struct SampleAudioData : IEquatable<SampleAudioData>
    {
        public float MasterVolume;
        public float MusicVolume;
        public float SfxVolume;
        public bool IsMuted;

        public SampleAudioData(float masterVolume, float musicVolume, float sfxVolume, bool isMuted = false)
        {
            MasterVolume = masterVolume;
            MusicVolume = musicVolume;
            SfxVolume = sfxVolume;
            IsMuted = isMuted;
        }

        public bool Equals(SampleAudioData other) =>
            MasterVolume.Equals(other.MasterVolume) &&
            MusicVolume.Equals(other.MusicVolume) &&
            SfxVolume.Equals(other.SfxVolume) &&
            IsMuted == other.IsMuted;

        public override bool Equals(object obj) => obj is SampleAudioData other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(MasterVolume, MusicVolume, SfxVolume, IsMuted);
    }
}
