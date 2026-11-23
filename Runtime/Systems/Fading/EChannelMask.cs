using System;

namespace MA.Systems.Fading
{
    [Flags]
    public enum EChannelMask
    {
        None = 0,
        Channel_0 = 1 << 0,
        Channel_1 = 1 << 1,
        Channel_2 = 1 << 2,
        Channel_3 = 1 << 3,
        Channel_4 = 1 << 4,
        Channel_5 = 1 << 5,
        Channel_6 = 1 << 6,
        Channel_7 = 1 << 7,
        All = ~0
    }
}
