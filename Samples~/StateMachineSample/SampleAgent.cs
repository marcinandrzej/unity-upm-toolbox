using UnityEngine;

namespace MA.Patterns.State.Samples
{
    public class SampleAgent : MonoBehaviour
    {
        private SampleStateMachine stateMachine;

        public float Timer { get; set; } = 0;

        public float SingingTimer { get; set; } = 0;

        public float WaitingTime { get; private set; }

        public float SingingTime { get; private set; }

        public float SingingFrequency { get; private set; }

        private void Awake()
        {
            WaitingTime = Random.Range(2, 5);
            SingingTime = Random.Range(3, 6);
            SingingFrequency = Random.Range(1, 2);
        }

        private void Start()
        {
            stateMachine = SampleStateMachineFactorySingleton.Instance.SampleStateMachineFactory.Create();
            stateMachine.Start(this);
        }

        private void Update()
        {
            stateMachine.Update(this);
        }

        public void LogMessage(string message) => Debug.Log($"Agent of entity ID : {GetEntityId().ToString()} send message {message}");
    }
}
