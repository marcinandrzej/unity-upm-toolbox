using System;
using System.Collections.Generic;

namespace MA.Patterns.State
{
    public abstract class StateGeneric<StateT, AgentT> where StateT : StateGeneric<StateT, AgentT>
    {
        private Action<StateT, AgentT> OnEnter = null;
        private Action<StateT, AgentT> OnExit = null;
        private Action<AgentT> OnUpdate = null;

        private List<StateTransitionGeneric<StateT, AgentT>> transitions = new List<StateTransitionGeneric<StateT, AgentT>>();

        public StateGeneric()
        {
            OnEnter = (_, _) => { };
            OnExit = (_, _) => { };
            OnUpdate = (_) => { };
        }

        public void RegisterTransition(StateTransitionGeneric<StateT, AgentT> transition)
        {
            transitions.Add(transition);
            transitions.Sort((a, b) => b.Priority.CompareTo(a.Priority));
        }

        internal void SetOnEnter(Action<StateT, AgentT> action) => OnEnter = action ?? ((_, _) => { });

        internal void SetOnExit(Action<StateT, AgentT> action) => OnExit = action ?? ((_, _) => { });

        internal void SetOnUpdate(Action<AgentT> action) => OnUpdate = action ?? ((_) => { });

        internal void Enter(StateT previousState, AgentT obj) => OnEnter.Invoke(previousState, obj);

        internal void Exit(StateT nextState, AgentT obj) => OnExit.Invoke(nextState, obj);

        internal void Update(AgentT obj) => OnUpdate.Invoke(obj);

        internal StateT CheckTransitions(AgentT obj)
        {
            for (int i = 0; i < transitions.Count; i++)
                if (transitions[i].CheckCondition(obj))
                    return transitions[i].ToState;

            return null;
        }
    }
}
