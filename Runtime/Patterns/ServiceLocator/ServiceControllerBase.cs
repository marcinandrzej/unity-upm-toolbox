using UnityEngine;

namespace MA.Patterns.ServiceLocator
{
    public abstract class ServiceControllerBase : MonoBehaviour
    {
        public abstract void RegisterService();

        public abstract void UnregisterService();
    }
}
