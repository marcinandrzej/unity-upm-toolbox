using Unity.Scripting.LifecycleManagement;
using UnityEngine;

namespace MA.Systems.Fading
{
    [NoAutoStaticsCleanup]
    public class LinearInterpolation : IFadeInterpolation
    {
        public static readonly LinearInterpolation Default = new();

        public float Evaluate(float time) => Mathf.Clamp01(time);
    }
}
