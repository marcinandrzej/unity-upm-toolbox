using MA.Patterns.Factory;

namespace MA.Patterns.State
{
    public abstract class StateMachineFactoryGeneric<StateT, AgentT, StateMachineT> : IFactoryGeneric<StateMachineT>
        where StateT : StateGeneric<StateT, AgentT>
        where StateMachineT : StateMachineGeneric<StateT, AgentT>
    {
        private bool isInitialized = false;

        public StateMachineT Create()
        {
            if (!isInitialized)
                Initialize();

            return CreateStateMachine();
        }

        public void Initialize()
        {
            if (isInitialized)
                return;

            CreateStates();
            CreateTransitions();

            isInitialized = true;
        }

        protected abstract void CreateStates();

        protected abstract void CreateTransitions();

        protected abstract StateMachineT CreateStateMachine();
    }
}
