using System.Collections.Generic;
using System.Linq;

namespace MA.Patterns.State
{
    public abstract class StateMachineGeneric<StateT, AgentT> where StateT : StateGeneric<StateT, AgentT>
    {
        private HashSet<StateT> states = new HashSet<StateT>();
        private StateT currentState = null;
        private StateT startingState = null;

        public void Start(AgentT agent)
        {
            StateT targetState = startingState ?? (states.Count > 0 ? states.First() : null);

            if (targetState != null)
                ChangeState(targetState, agent);
        }

        public void Update(AgentT agent)
        {
            if (currentState == null)
                return;

            StateT targetState = currentState.CheckTransitions(agent);

            if (targetState != null)
                ChangeState(targetState, agent);
            else
                currentState.Update(agent);
        }

        public void RegisterState(StateT state, bool isStarting = false)
        {
            if (states.Add(state) && (isStarting || startingState == null))
                startingState = state;
        }

        private void ChangeState(StateT nextState, AgentT agent)
        {
            currentState?.Exit(nextState, agent);
            nextState.Enter(currentState, agent);
            currentState = nextState;
        }
    }
}

