using MA.Patterns.Singleton;

namespace MA.Patterns.State.Samples
{
    public class SampleStateMachineFactorySingleton : SingletonMonoGeneric<SampleStateMachineFactorySingleton, SampleStateMachineFactorySingleton>
    {
        public SampleStateMachineFactory SampleStateMachineFactory { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            SampleStateMachineFactory = new SampleStateMachineFactory();
        }
    }
}
