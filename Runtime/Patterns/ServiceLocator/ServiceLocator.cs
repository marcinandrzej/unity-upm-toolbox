using MA.Patterns.SingletonSpace;
using System;
using System.Collections.Generic;

namespace MA.Patterns.ServiceLocatorSpace
{
    public class ServiceLocator : SingletonMonoGeneric<IServiceLocator, ServiceLocator>, IServiceLocator
    {
        private Dictionary<Type, object> _typeToServiceDict = new Dictionary<Type, object>();

        protected override void Awake()
        {
            base.Awake();

            if (Instance != this as IServiceLocator) 
                return;

            if (_typeToServiceDict != null)
                _typeToServiceDict.Clear();
            else
                _typeToServiceDict = new Dictionary<Type, object>();
        }

        private void OnDestroy()
        {
            if (_typeToServiceDict != null)
                _typeToServiceDict.Clear();
        }

        public EServiceOperationResult TryRegisterService<ServiceInterfaceT>(ServiceInterfaceT service)
        {
            Type type = typeof(ServiceInterfaceT);

            if (_typeToServiceDict.TryAdd(type, service))
                return EServiceOperationResult.Success;
            else
                return EServiceOperationResult.Failure | EServiceOperationResult.ServiceAlreadyRegistered;
        }

        public EServiceOperationResult TryUnregisterService<ServiceInterfaceT>()
        {
            Type type = typeof(ServiceInterfaceT);

            if (_typeToServiceDict.Remove(type))
                return EServiceOperationResult.Success;
            else
                return EServiceOperationResult.Failure | EServiceOperationResult.ServiceNotRegistered;
        }

        public EServiceOperationResult TryGetService<ServiceInterfaceT>(out ServiceInterfaceT service)
        {
            Type type = typeof(ServiceInterfaceT);

            if (_typeToServiceDict.TryGetValue(type, out var serviceObject))
            {
                service = (ServiceInterfaceT)serviceObject;

                return EServiceOperationResult.Success;
            }
            else
            {
                service = default;

                return EServiceOperationResult.Failure | EServiceOperationResult.ServiceNotRegistered;
            }
        }
    }
}
