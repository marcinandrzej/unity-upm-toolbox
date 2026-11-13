namespace MA.Patterns.ServiceLocator
{
    public interface IServiceLocator
    {
        public EServiceOperationResult TryRegisterService<ServiceInterfaceT>(ServiceInterfaceT service);

        public EServiceOperationResult TryUnregisterService<ServiceInterfaceT>();

        public EServiceOperationResult TryGetService<ServiceInterfaceT>(out ServiceInterfaceT service);
    }
}
