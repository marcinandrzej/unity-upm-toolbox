using System;

namespace MA.Serialization
{
    [Flags]
    public enum ESerializationOperationResult
    {
        None = 0,
        Success = 1 << 0,
        Failure = 1 << 1,
        Unknown = 1 << 2,
        DirectoryNotFound = 1 << 3,
        FileNotFound = 1 << 4,
        UnauthorizedAccess = 1 << 5,
        PathTooLong = 1 << 6,
        CryptographicError = 1 << 7,        
        NotEnoughSpace = 1 << 8,
    }
}
