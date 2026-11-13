using System.Threading;
using UnityEngine;

namespace MA.Patterns.State.Samples
{
    public class SampleStateMachineFactory : StateMachineFactoryGeneric<SampleState, SampleAgent, SampleStateMachine>
    {
        private SampleState _waitingState;
        private SampleState _singingState;

        private SampleStateTransition _waitToSingtransition;
        private SampleStateTransition _singToWaittransition;

        protected override void CreateStates()
        {
            SampleStateBuilder builder = new SampleStateBuilder();

            _waitingState = builder
            .WithOnEnter((SampleState state, SampleAgent agent) => 
            {
                agent.LogMessage("Enter waiting state");
                agent.Timer = 0;
            })
            .WithOnUpdate((SampleAgent agent) => { agent.Timer += Time.deltaTime; })
            .WithOnExit((SampleState state, SampleAgent agent) => 
            {
                agent.Timer = 0;
                agent.LogMessage("Exit waiting state");
            })
            .Build();

            builder.Reset();

            _singingState = builder
            .WithOnEnter((SampleState state, SampleAgent agent) => 
            {
                agent.LogMessage("Enter singing state");
                agent.Timer = 0;
                agent.SingingTimer = 0;
            })
            .WithOnUpdate((SampleAgent agent) => 
            {
                agent.Timer += Time.deltaTime;
                agent.SingingTimer += Time.deltaTime;
                if (agent.SingingTimer > agent.SingingFrequency) 
                {
                    agent.LogMessage("Singing");
                    agent.SingingTimer -= agent.SingingFrequency;
                }
            })
            .WithOnExit((SampleState state, SampleAgent agent) => 
            {
                agent.Timer = 0;
                agent.SingingTimer = 0;
                agent.LogMessage("Exit singing state"); 
            })
            .Build();
        }

        protected override void CreateTransitions()
        {
            _waitToSingtransition = new SampleStateTransition(_singingState, (SampleAgent agent) => agent.Timer >= agent.WaitingTime, 0);
            _waitingState.RegisterTransition(_waitToSingtransition);
            
            _singToWaittransition = new SampleStateTransition(_waitingState, (SampleAgent agent) => agent.Timer >= agent.SingingTime, 0);
            _singingState.RegisterTransition(_singToWaittransition);
        }

        protected override SampleStateMachine CreateStateMachine()
        {
            SampleStateMachine machine = new SampleStateMachine();

            machine.RegisterState(_waitingState, true);
            machine.RegisterState(_singingState);

            return machine;
        }


    }
}
