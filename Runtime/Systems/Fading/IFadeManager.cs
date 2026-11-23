using System;

namespace MA.Systems.Fading
{
    public interface IFadeManager
    {
        public EFadeOperationResult TryRegisterListener(IFadeListener listener);

        public EFadeOperationResult TryUnregisterListener(IFadeListener listener);

        public EFadeOperationResult TryRegisterChannel(EChannelMask channelMask, ETimeMode timeMode, bool visible);

        public EFadeOperationResult TryUnregisterChannel(EChannelMask channelMask);

        public EFadeOperationResult TryClearChannel(EChannelMask channelMask);

        public EFadeOperationResult TryShowChannel(EChannelMask channelMask);

        public EFadeOperationResult TryHideChannel(EChannelMask channelMask);

        public EFadeOperationResult TryFadeIn(EChannelMask channelMask, int id, float duration, IFadeInterpolation interpolation, Action onCompletedCallback = null, Action onTerminatedCallback = null);
        
        public EFadeOperationResult TryFadeOut(EChannelMask channelMask, int id, float duration, IFadeInterpolation interpolation, Action onCompletedCallback = null, Action onTerminatedCallback = null);

        public EFadeOperationResult TryFadeTerminate(EChannelMask channelMask, int id);
    }
}
