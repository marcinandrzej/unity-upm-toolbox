using System;
using System.Collections.Generic;
using UnityEngine;

namespace MA.Systems.Fading
{
    [Serializable]
    public class FadeChannel
    {
        [SerializeField] private bool visible;
        [SerializeField] private ETimeMode timeMode;

        private float _alpha;
        private bool _alphaChanged;
        private EChannelUpdate _updateFlag;
        private readonly Stack<Fade> _fadesPool = new();
        private readonly Dictionary<int, Fade> _idToFadeDict = new();
        private readonly List<int> _fadingList = new();
        private readonly List<int> _invalidBufferList = new();
        private readonly List<(int id, Fade fade)> _inactiveBufferList = new();
        private readonly List<(int id, Fade fade)> _completedBufferList = new();
        private readonly List<Action> _callbacksBufferList = new();

        public float Alpha => visible ? _alpha : 0f;

        public bool HasAlphaChanged => _alphaChanged;

        public bool RequiresUpdate => _fadingList.Count > 0 || (_updateFlag != EChannelUpdate.None);

        public FadeChannel()
        {
            visible = true;
            timeMode = ETimeMode.Unscaled;
        }

        public FadeChannel(ETimeMode timeMode, bool visible)
        {
            this.visible = visible;
            this.timeMode = timeMode;
        }

        public bool TryShow() 
        {
            if (visible)
                return false;

            visible = true;
            _updateFlag |= EChannelUpdate.VisibilityChanged;

            return true;
        }

        public bool TryHide()
        {
            if (!visible)
                return false;

            visible = false;
            _updateFlag |= EChannelUpdate.VisibilityChanged;

            return true;
        }

        public bool TryClear() 
        {
            if(_idToFadeDict.Count == 0)
                return false;

            List<Action> callbacks = new List<Action>();

            foreach (KeyValuePair<int, Fade> idToFade in _idToFadeDict)
            {
                Fade fade = idToFade.Value;
                Action onTerminated = fade.OnTerminated;

                if (onTerminated != null)
                    callbacks.Add(onTerminated);

                fade.ResetData();
                _fadesPool.Push(fade);
            }

            _idToFadeDict.Clear();
            _fadingList.Clear();            
            _updateFlag |= EChannelUpdate.HasBeenCleared;

            foreach (Action callback in callbacks)
                callback.Invoke();

            return true;
        }

        public EFadeOperationResult TryFadeIn(int id, float duration, IFadeInterpolation interpolation, Action onCompletedCallback = null, Action onTerminatedCallback = null) 
        {
            if (_idToFadeDict.TryGetValue(id, out Fade fade)) 
            {
                switch (fade.State)
                {
                    case EFadeState.FadingIn:
                        return EFadeOperationResult.Failure | EFadeOperationResult.FadeIsFadingIn;
                    case EFadeState.Active:
                        return EFadeOperationResult.Failure | EFadeOperationResult.FadeIsActive;
                    case EFadeState.FadingOut:
                        return EFadeOperationResult.Failure | EFadeOperationResult.FadeIsFadingOut;
                    default:
                        return EFadeOperationResult.Failure | EFadeOperationResult.Unknown;
                }
            }
            else
            {
                fade = GetOrCreateFade(id);
                EFadeOperationResult result = fade.TryFadeIn(duration, interpolation, onCompletedCallback, onTerminatedCallback);

                if (result.HasFlag(EFadeOperationResult.Failure)) 
                {
                    fade.ResetData();
                    _fadesPool.Push(fade);
                    fade = null;

                    return result;
                }

                if (!_idToFadeDict.TryAdd(id, fade)) 
                {
                    fade.ResetData();
                    _fadesPool.Push(fade);
                    fade = null;

                    return EFadeOperationResult.Failure | EFadeOperationResult.Unknown;
                }

                if(!_fadingList.Contains(id))
                    _fadingList.Add(id);

                return EFadeOperationResult.Success;
            }
        }

        public EFadeOperationResult TryFadeOut(int id, float duration, IFadeInterpolation interpolation, Action onCompletedCallback = null, Action onTerminatedCallback = null) 
        {
            if (!_idToFadeDict.TryGetValue(id, out Fade fade))
                return EFadeOperationResult.Failure | EFadeOperationResult.FadeIsInactive;

            if (fade.State != EFadeState.Active) 
            {
                if (fade.State == EFadeState.FadingIn)
                    return EFadeOperationResult.Failure | EFadeOperationResult.FadeIsFadingIn;

                if (fade.State == EFadeState.FadingOut)
                    return EFadeOperationResult.Failure | EFadeOperationResult.FadeIsFadingOut;

                return EFadeOperationResult.Failure | EFadeOperationResult.Unknown;
            }
            else
            {
                EFadeOperationResult result = fade.TryFadeOut(duration, interpolation, onCompletedCallback, onTerminatedCallback);

                if (result.HasFlag(EFadeOperationResult.Failure))
                {
                    fade = null;

                    return result;
                }

                if (!_fadingList.Contains(id))
                    _fadingList.Add(id);

                return EFadeOperationResult.Success;
            }
        }

        public EFadeOperationResult TryFadeTerminate(int id) 
        {
            if (!_idToFadeDict.TryGetValue(id, out Fade fade))
                return EFadeOperationResult.Failure | EFadeOperationResult.FadeIsInactive;

            Action onTerminated = fade.OnTerminated;

            _fadingList.Remove(id);
            _idToFadeDict.Remove(id);
            fade.ResetData();
            _fadesPool.Push(fade);
            _updateFlag |= EChannelUpdate.FadeTerminated;

            onTerminated?.Invoke();

            return EFadeOperationResult.Success;
        }

        public void UpdateChannel()
        {
            float previousAlpha = _updateFlag.HasFlag(EChannelUpdate.VisibilityChanged) ? (visible ? 0 : _alpha) : Alpha;
            float deltaTime = timeMode == ETimeMode.Unscaled ? Time.unscaledDeltaTime : Time.deltaTime;

            UpdateFades(deltaTime);

            _alpha = CalculateAlpha();
            _alphaChanged = !Mathf.Approximately(previousAlpha, Alpha);
            _updateFlag = EChannelUpdate.None;
        }

        private void UpdateFades(float deltaTime) 
        {
            _invalidBufferList.Clear();
            _inactiveBufferList.Clear();
            _completedBufferList.Clear();
            _callbacksBufferList.Clear();

            foreach (int id in _fadingList)
            {
                if (_idToFadeDict.TryGetValue(id, out Fade fade))
                {
                    fade.UpdateFade(deltaTime);

                    if (fade.State == EFadeState.Active)
                    {
                        _completedBufferList.Add((id, fade));
                    }
                    else if (fade.State == EFadeState.Inactive)
                    {
                        _inactiveBufferList.Add((id, fade));
                    }
                }
                else
                {
                    _invalidBufferList.Add(id);
                }
            }

            foreach (int id in _invalidBufferList)
                _fadingList.Remove(id);

            foreach ((int id, Fade fade) record in _completedBufferList)
            {
                Action onCompleted = record.fade.OnCompleted;

                if (onCompleted != null)
                    _callbacksBufferList.Add(onCompleted);

                _fadingList.Remove(record.id);
                record.fade.ResetCallbacks();
            }

            foreach ((int id, Fade fade) record in _inactiveBufferList)
            {
                Action onCompleted = record.fade.OnCompleted;

                if (onCompleted != null)
                    _callbacksBufferList.Add(onCompleted);

                record.fade.ResetData();

                _fadingList.Remove(record.id);
                _idToFadeDict.Remove(record.id);
                _fadesPool.Push(record.fade);
            }

            foreach (Action callback in _callbacksBufferList)
                callback.Invoke();
        }

        private float CalculateAlpha()
        {
            float maxAlpha = 0f;

            foreach (Fade fade in _idToFadeDict.Values)
                if (fade.Alpha > maxAlpha)
                    maxAlpha = fade.Alpha;

            return maxAlpha;
        }

        private Fade GetOrCreateFade(int id)
        {
            if (!_fadesPool.TryPop(out Fade fade))
                fade = new Fade();

            fade.SetData(id);

            return fade;
        }
    }
}
