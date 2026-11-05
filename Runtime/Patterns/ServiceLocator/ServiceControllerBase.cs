using UnityEngine;

namespace MA.Patterns.ServiceLocatorSpace
{
    public abstract class ServiceControllerBase : MonoBehaviour
    {
        public abstract void RegisterService();

        public abstract void UnregisterService();
    }
}
