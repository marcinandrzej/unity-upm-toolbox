using System;

namespace MA.Patterns.ServiceLocatorSpace
{
    [Flags]
    public enum EServiceOperationResult
    {
        None = 0,
        Success = 1 << 0,
        Failure = 1 << 1,
        ServiceAlreadyRegistered = 1 << 2,
        ServiceNotRegistered = 1 << 3,
    }
}
