using System;

namespace MA.Systems.Fading
{
    [Flags]
    public enum EFadeOperationResult
    {
        None = 0,
        Success = 1 << 0,
        Failure = 1 << 1,
        Unknown = 1 << 2,
        InvalidData = 1 << 3,
        FadeIsInactive = 1 << 4,
        FadeIsFadingIn = 1 << 5,
        FadeIsActive = 1 << 6,
        FadeIsFadingOut = 1 << 7,
        ChannelIsRegistered = 1 << 8,
        ChannelIsNotRegistered = 1 << 9,
        ChannelIsShown = 1 << 10,
        ChannelIsHidden = 1 << 11,
        ChannelIsClean = 1 << 12,
        ListenerIsRegistered = 1 << 13,
        ListenerIsNotRegistered = 1 << 14
    }
}
