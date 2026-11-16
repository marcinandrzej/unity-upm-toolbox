using System.Threading;
using UnityEngine;

namespace MA.Systems.Transition
{
    public interface ITransition
    {
        public Awaitable Execute(CancellationToken ct = default);
    }
}
