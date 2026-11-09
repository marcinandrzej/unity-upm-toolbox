using System.Collections;

namespace MA.Systems.Transition
{
    public interface ITransition
    {
        public IEnumerator Execute();
    }
}
