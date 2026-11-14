using System;
using UnityEngine;

namespace MA.Systems.Settings.Samples
{
    [Serializable]
    public struct SampleResolutionData : IEquatable<SampleResolutionData>
    {
        public int Width;
        public int Height;
        public FullScreenMode ScreenMode;

        public SampleResolutionData(int width, int height, FullScreenMode screenMode)
        {
            Width = width;
            Height = height;
            ScreenMode = screenMode;
        }

        public bool Equals(SampleResolutionData other) => Width == other.Width && Height == other.Height && ScreenMode == other.ScreenMode;

        public override bool Equals(object obj) => obj is SampleResolutionData other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(Width, Height, ScreenMode);

        public override string ToString() => $"{Width}x{Height} ({ScreenMode})";
    }
}
