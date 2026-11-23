using Unity.Scripting.LifecycleManagement;
using UnityEngine;

namespace MA.Systems.Fading
{
    [NoAutoStaticsCleanup]
    public class EaseOutQuadInterpolation : IFadeInterpolation
    {
        public static readonly EaseOutQuadInterpolation Default = new();

        public float Evaluate(float time)
        {
            time = Mathf.Clamp01(time);

            return 1f - (1f - time) * (1f - time);
        }
    }
}
