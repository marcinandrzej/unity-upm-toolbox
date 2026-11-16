using UnityEngine;

namespace MA.Systems.Transition
{
    public interface ITransitionManager
    {
        public bool IsInTransition { get; }

        public Awaitable ExecuteStateTransition(ITransition transition);
    }
}
