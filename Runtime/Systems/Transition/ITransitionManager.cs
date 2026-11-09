namespace MA.Systems.Transition
{
    public interface ITransitionManager
    {
        public bool IsInTransition { get; }

        public void ExecuteStateTransition(ITransition transition);
    }
}
