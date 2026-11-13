using UnityEngine;

namespace MA.Patterns.ServiceLocator
{
    public abstract class ServiceControllerGeneric<ServiceInterfaceT, ServiceClassT> : ServiceControllerBase where ServiceClassT : class, ServiceInterfaceT
    {
        [SerializeField] private ServiceClassT service;

        public override void RegisterService()
        {
            if (ServiceLocatorMono.Instance == null)
            {
                Debug.LogError("ServiceLocator is null");

                return;
            }

            EServiceOperationResult result = ServiceLocatorMono.Instance.TryRegisterService<ServiceInterfaceT>(service);

            if (result.HasFlag(EServiceOperationResult.Failure))
                Debug.LogError($"Register service of type [{typeof(ServiceInterfaceT).Name}] fails with result [{result.ToString()}]");
        }
        public override void UnregisterService()
        {
            if (ServiceLocatorMono.Instance == null)
            {
                Debug.LogError("ServiceLocator is null");

                return;
            }

            EServiceOperationResult result = ServiceLocatorMono.Instance.TryUnregisterService<ServiceInterfaceT>();

            if (result.HasFlag(EServiceOperationResult.Failure))
                Debug.LogError($"Unregister service of type [{typeof(ServiceInterfaceT).Name}] fails with result [{result.ToString()}]");
        }
    }
}
