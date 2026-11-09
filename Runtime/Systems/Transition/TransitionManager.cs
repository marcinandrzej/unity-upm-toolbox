using System.Collections;
using UnityEngine;

namespace MA.Systems.Transition
{
    public class TransitionManager : MonoBehaviour, ITransitionManager
    {
        private Coroutine _currentTransition = null;

        public bool IsInTransition => _currentTransition != null;

        public void ExecuteStateTransition(ITransition transition)
        {
            if (IsInTransition)
            {
                Debug.LogError("TransitionManager is already in transition");

                return;
            }

            _currentTransition = StartCoroutine(ExecuteStateTransitionCoroutine(transition));
        }

        private IEnumerator ExecuteStateTransitionCoroutine(ITransition transition)
        {
            yield return transition.Execute();

            _currentTransition = null;
        }
    }
}
