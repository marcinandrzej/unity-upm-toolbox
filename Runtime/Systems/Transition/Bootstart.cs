using UnityEngine;

namespace MA.Systems.Transition
{
    public class Bootstart : MonoBehaviour
    {
        [Tooltip("Reference to ITransitionManager")]
        [SerializeField] private Object transitionManagerObject = null;

        [Tooltip("Reference to initial ITransition")]
        [SerializeField] private Object transitionObject = null;

        private void OnValidate()
        {
            if (transitionManagerObject != null && !(transitionManagerObject is ITransitionManager))
                transitionManagerObject = null;

            if (transitionObject != null && !(transitionObject is ITransition))
                transitionObject = null;
        }

        private void Start()
        {
            ITransitionManager manager = transitionManagerObject as ITransitionManager;
            ITransition transition = transitionObject as ITransition;
            manager.ExecuteStateTransition(transition);
            Destroy(gameObject);
        }
    }
}
