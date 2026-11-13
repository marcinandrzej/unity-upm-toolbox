using System;

namespace MA.Patterns.State
{
    public abstract class StateTransitionGeneric<StateT, AgentT> where StateT : StateGeneric<StateT, AgentT>
    {
        private readonly Func<AgentT, bool> Condition;

        public int Priority { get; private set; }

        public StateT ToState { get; private set; }

        public StateTransitionGeneric(StateT toState, Func<AgentT, bool> condition, int priority)
        {
            Priority = priority;
            ToState = toState;
            Condition = condition;
        }

        public bool CheckCondition(AgentT obj) => Condition.Invoke(obj);
    }
}
