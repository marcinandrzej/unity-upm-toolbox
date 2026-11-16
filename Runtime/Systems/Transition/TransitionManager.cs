using System;
using UnityEngine;

namespace MA.Systems.Transition
{
    public class TransitionManager : MonoBehaviour, ITransitionManager
    {
        public bool IsInTransition { get; private set; } = false;

        public async Awaitable ExecuteStateTransition(ITransition transition)
        {
            if (IsInTransition)
            {
                Debug.LogError("TransitionManager is already in transition.", this);

                return;
            }

            if (transition == null)
            {
                Debug.LogError("Transition cannot be null.", this);

                return;
            }

            IsInTransition = true;

            try
            {
                await transition.Execute(destroyCancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Expected when TransitionManager is destroyed or Play Mode exits
            }
            catch (Exception ex)
            {
                Debug.LogException(ex, this);
            }
            finally
            {
                IsInTransition = false;
            }
        }
    }
}
