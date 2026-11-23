using System;
using System.Collections.Generic;
using UnityEngine;

namespace MA.Systems.Fading
{
    public class FadeManager : MonoBehaviour, IFadeManager
    {
        [SerializeField] private Dictionary<EChannelMask, FadeChannel> maskToChannelDict = new ();

        private List<IFadeListener> _listenersList = new ();

        private void Update()
        {
            EChannelMask changedChannels = EChannelMask.None;
            bool updateRequired = false;

            foreach (KeyValuePair<EChannelMask, FadeChannel> maskToChannel in maskToChannelDict) 
            {
                EChannelMask channelMask = maskToChannel.Key;
                FadeChannel channel = maskToChannel.Value;

                if (channel.RequiresUpdate)
                {
                    channel.UpdateChannel();

                    if (channel.HasAlphaChanged)
                        changedChannels |= channelMask;

                    updateRequired = true;
                }
            }

            if (changedChannels != EChannelMask.None)
            {
                foreach (IFadeListener listener in _listenersList)
                {
                    if (listener == null)
                        continue;

                    if ((listener.ChannelMask & changedChannels) != 0)
                    {
                        float alpha = GetMaxAlpha(listener.ChannelMask);
                        listener.ApplyAlpha(alpha);
                    }
                }
            }

            if (!updateRequired)
                Disable();
        }

        public EFadeOperationResult TryRegisterListener(IFadeListener listener) 
        {
            if (listener == null)
                return EFadeOperationResult.Failure | EFadeOperationResult.InvalidData;

            if (_listenersList.Contains(listener))
                return EFadeOperationResult.Failure | EFadeOperationResult.ListenerIsRegistered;

            _listenersList.Add(listener);

            float alpha = GetMaxAlpha(listener.ChannelMask);
            listener.ApplyAlpha(alpha);

            return EFadeOperationResult.Success;
        }

        public EFadeOperationResult TryUnregisterListener(IFadeListener listener)
        {
            if (listener == null)
                return EFadeOperationResult.Failure | EFadeOperationResult.InvalidData;

            if (!_listenersList.Remove(listener))
                return EFadeOperationResult.Failure | EFadeOperationResult.ListenerIsNotRegistered;

            listener.ApplyAlpha(0);

            return EFadeOperationResult.Success;
        }

        public EFadeOperationResult TryRegisterChannel(EChannelMask channelMask, ETimeMode timeMode, bool visible) 
        {
            if(!channelMask.HasSingleFlag())
                return EFadeOperationResult.Failure | EFadeOperationResult.InvalidData;

            if(maskToChannelDict.ContainsKey(channelMask))
                return EFadeOperationResult.Failure | EFadeOperationResult.ChannelIsRegistered;

            FadeChannel channel = new FadeChannel(timeMode, visible);

            if (!maskToChannelDict.TryAdd(channelMask, channel))
                return EFadeOperationResult.Failure | EFadeOperationResult.Unknown;

            return EFadeOperationResult.Success;
        }

        public EFadeOperationResult TryUnregisterChannel(EChannelMask channelMask)
        {
            if (!channelMask.HasSingleFlag())
                return EFadeOperationResult.Failure | EFadeOperationResult.InvalidData;

            if (!maskToChannelDict.Remove(channelMask))
                return EFadeOperationResult.Failure | EFadeOperationResult.ChannelIsNotRegistered;

            return EFadeOperationResult.Success;
        }

        public EFadeOperationResult TryClearChannel(EChannelMask channelMask) 
        {
            if (!channelMask.HasSingleFlag())
                return EFadeOperationResult.Failure | EFadeOperationResult.InvalidData;

            if (!maskToChannelDict.TryGetValue(channelMask, out FadeChannel channel))
                return EFadeOperationResult.Failure | EFadeOperationResult.ChannelIsNotRegistered;

            if (!channel.TryClear())
                return EFadeOperationResult.Failure | EFadeOperationResult.ChannelIsClean;

            Enable();

            return EFadeOperationResult.Success;
        }

        public EFadeOperationResult TryShowChannel(EChannelMask channelMask) 
        {
            if (!channelMask.HasSingleFlag())
                return EFadeOperationResult.Failure | EFadeOperationResult.InvalidData;

            if (!maskToChannelDict.TryGetValue(channelMask, out FadeChannel channel))
                return EFadeOperationResult.Failure | EFadeOperationResult.ChannelIsNotRegistered;

            if(!channel.TryShow())
                return EFadeOperationResult.Failure | EFadeOperationResult.ChannelIsShown;

            Enable();

            return EFadeOperationResult.Success;
        }

        public EFadeOperationResult TryHideChannel(EChannelMask channelMask) 
        {
            if (!channelMask.HasSingleFlag())
                return EFadeOperationResult.Failure | EFadeOperationResult.InvalidData;

            if (!maskToChannelDict.TryGetValue(channelMask, out FadeChannel channel))
                return EFadeOperationResult.Failure | EFadeOperationResult.ChannelIsNotRegistered;

            if (!channel.TryHide())
                return EFadeOperationResult.Failure | EFadeOperationResult.ChannelIsHidden;

            Enable();

            return EFadeOperationResult.Success;
        }

        public EFadeOperationResult TryFadeIn(EChannelMask channelMask, int id, float duration, IFadeInterpolation interpolation, Action onCompletedCallback = null, Action onTerminatedCallback = null) 
        {
            if (!channelMask.HasSingleFlag())
                return EFadeOperationResult.Failure | EFadeOperationResult.InvalidData;

            if (!maskToChannelDict.TryGetValue(channelMask, out FadeChannel channel))
                return EFadeOperationResult.Failure | EFadeOperationResult.ChannelIsNotRegistered;

            EFadeOperationResult result = channel.TryFadeIn(id, duration, interpolation, onCompletedCallback, onTerminatedCallback);

            if (result.HasFlag(EFadeOperationResult.Failure))
                return result;

            Enable();

            return EFadeOperationResult.Success;
        }

        public EFadeOperationResult TryFadeOut(EChannelMask channelMask, int id, float duration, IFadeInterpolation interpolation, Action onCompletedCallback = null, Action onTerminatedCallback = null) 
        {
            if (!channelMask.HasSingleFlag())
                return EFadeOperationResult.Failure | EFadeOperationResult.InvalidData;

            if (!maskToChannelDict.TryGetValue(channelMask, out FadeChannel channel))
                return EFadeOperationResult.Failure | EFadeOperationResult.ChannelIsNotRegistered;

            EFadeOperationResult result = channel.TryFadeOut(id, duration, interpolation, onCompletedCallback, onTerminatedCallback);

            if (result.HasFlag(EFadeOperationResult.Failure))
                return result;

            Enable();

            return EFadeOperationResult.Success;
        }

        public EFadeOperationResult TryFadeTerminate(EChannelMask channelMask, int id) 
        {
            if (!channelMask.HasSingleFlag())
                return EFadeOperationResult.Failure | EFadeOperationResult.InvalidData;

            if (!maskToChannelDict.TryGetValue(channelMask, out FadeChannel channel))
                return EFadeOperationResult.Failure | EFadeOperationResult.ChannelIsNotRegistered;

            EFadeOperationResult result = channel.TryFadeTerminate(id);

            if (result.HasFlag(EFadeOperationResult.Failure))
                return result;

            Enable();

            return EFadeOperationResult.Success;
        }

        private void Enable()
        {
            if(!enabled)
                enabled = true;
        }

        private void Disable()
        {
            if (enabled)
                enabled = false;
        }

        private float GetMaxAlpha(EChannelMask subscribedMask)
        {
            float maxAlpha = 0f;

            foreach (KeyValuePair<EChannelMask, FadeChannel> maskToChannel in maskToChannelDict)
            {
                if ((maskToChannel.Key & subscribedMask) == 0)
                    continue;

                FadeChannel channel = maskToChannel.Value;

                if (channel.Alpha > maxAlpha)
                    maxAlpha = channel.Alpha;
            }

            return maxAlpha;
        }

#if UNITY_EDITOR
        [ContextMenu("DebugFadeIn")]
        public void DebugFadeIn() => TryFadeIn(EChannelMask.Channel_0, 89, 2, LinearInterpolation.Default);

        [ContextMenu("DebugFadeOut")]
        public void DebugFadeOut() => TryFadeOut(EChannelMask.Channel_0, 89, 2, LinearInterpolation.Default);

        [ContextMenu("DebugShowChannel")]
        public void DebugShowChannel() => TryShowChannel(EChannelMask.Channel_0);

        [ContextMenu("DebugHideChannel")]
        public void DebugHideChannel() => TryHideChannel(EChannelMask.Channel_0);

        [ContextMenu("DebugClearChannel")]
        public void DebugClearChannel() => TryClearChannel(EChannelMask.Channel_0);

        [ContextMenu("DebugFadeTerminate")]
        public void DebugTerminateFade() => TryFadeTerminate(EChannelMask.Channel_0, 89);
#endif
    }
}