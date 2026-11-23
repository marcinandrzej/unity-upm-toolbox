using Unity.Scripting.LifecycleManagement;
using UnityEngine;

namespace MA.Systems.Fading
{
    [NoAutoStaticsCleanup]
    public class EaseInQuadInterpolation : IFadeInterpolation
    {
        public static readonly EaseInQuadInterpolation Default = new();

        public float Evaluate(float time)
        {
            time = Mathf.Clamp01(time);

            return time * time;
        }
    }
}
