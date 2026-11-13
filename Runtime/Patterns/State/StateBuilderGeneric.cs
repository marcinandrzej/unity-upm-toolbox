using System;

namespace MA.Patterns.State
{
    public abstract class StateBuilderGeneric<StateT, AgentT, BuilderT> where StateT : StateGeneric<StateT, AgentT>, new() where BuilderT : StateBuilderGeneric<StateT, AgentT, BuilderT>
    {
        protected StateT state = null;

        public StateBuilderGeneric() => state = new StateT();

        public BuilderT WithOnEnter(Action<StateT, AgentT> onEnter)
        {
            state.SetOnEnter(onEnter);

            return this as BuilderT;
        }

        public BuilderT WithOnExit(Action<StateT, AgentT> onExit)
        {
            state.SetOnExit(onExit);

            return this as BuilderT;
        }

        public BuilderT WithOnUpdate(Action<AgentT> onUpdate)
        {
            state.SetOnUpdate(onUpdate);

            return this as BuilderT;
        }

        public StateT Build() => state;

        public StateT Reset() => state = new StateT();
    }
}
