using System;

namespace MA.Systems.Fading
{
    [Flags]
    public enum EChannelUpdate
    {
        None = 0,
        VisibilityChanged = 1 << 0,
        HasBeenCleared = 1 << 1,
        FadeTerminated = 1 << 2,
        All = ~0
    }
}
