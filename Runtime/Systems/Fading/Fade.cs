using System;
using UnityEngine;

namespace MA.Systems.Fading
{
    public class Fade
    {
        private const int InvalidID = -1;
        private const float MinValidDuration = 0.01f;

        private float _startAlpha;
        private float _targetAlpha;
        private float _duration;
        private float _elapsed;
        private IFadeInterpolation _interpolation;

        public Action OnCompleted { get; private set; } = null;

        public Action OnTerminated { get; private set; } = null;

        public int ID { get; private set; } = InvalidID;

        public float Alpha { get; private set; }

        public EFadeState State { get; private set; }

        public void SetData(int id)
        {
            ID = id;
            Alpha = 0f;
            State = EFadeState.Inactive;
        }

        public void ResetData()
        {
            ID = InvalidID;
            Alpha = 0f;
            State = EFadeState.Inactive;

            _interpolation = null;
            _duration = MinValidDuration;
            _elapsed = 0f;
            _startAlpha = 0f;
            _targetAlpha = 0f;

            OnCompleted = null;
            OnTerminated = null;
        }

        public void ResetCallbacks() 
        {
            OnCompleted = null;
            OnTerminated = null;
        }

        public EFadeOperationResult TryFadeIn(float duration, IFadeInterpolation interpolation = null, Action onCompletedCallback = null, Action onTerminatedCallback = null) 
        {
            if (duration <= MinValidDuration)
                return EFadeOperationResult.Failure | EFadeOperationResult.InvalidData;

            _interpolation = interpolation ?? LinearInterpolation.Default;
            _duration = duration;
            _elapsed = 0f;
            _startAlpha = 0;
            _targetAlpha = 1f;

            OnCompleted = onCompletedCallback;
            OnTerminated = onTerminatedCallback;

            State = EFadeState.FadingIn;

            return EFadeOperationResult.Success;
        }

        public EFadeOperationResult TryFadeOut(float duration, IFadeInterpolation interpolation = null, Action onCompletedCallback = null, Action onTerminatedCallback = null)
        {
            if (duration <= MinValidDuration)
                return EFadeOperationResult.Failure | EFadeOperationResult.InvalidData;

            _interpolation = interpolation ?? LinearInterpolation.Default;
            _duration = duration;
            _elapsed = 0f;
            _startAlpha = 1;
            _targetAlpha = 0f;

            OnCompleted = onCompletedCallback;
            OnTerminated = onTerminatedCallback;

            State = EFadeState.FadingOut;

            return EFadeOperationResult.Success;
        }

        public void UpdateFade(float deltaTime)
        {
            if (State != EFadeState.FadingIn && State != EFadeState.FadingOut)
                return;

            _elapsed += deltaTime;
            float time = Mathf.Clamp01(_elapsed / _duration);
            float curvedTime = _interpolation.Evaluate(time);

            Alpha = Mathf.Lerp(_startAlpha, _targetAlpha, curvedTime);

            if (time >= 1f)
            {
                Alpha = _targetAlpha;
                State = State == EFadeState.FadingIn ? EFadeState.Active : EFadeState.Inactive;
            }
        }
    }
}
