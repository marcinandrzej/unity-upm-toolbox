using System;

namespace MA.Patterns.State.Samples
{
    public class SampleStateTransition : StateTransitionGeneric<SampleState, SampleAgent>
    {
        public SampleStateTransition(SampleState toState, Func<SampleAgent, bool> condition, int priority) : base(toState, condition, priority)
        {

        }
    }
}
