using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

namespace MA.Serialization.Serialization
{
    [CreateAssetMenu(fileName = "SerializationExceptionHandler", menuName = "Toolbox/Serialization/Exception Handler/Default")]
    public class SerializationExceptionHandler : SerializationExceptionHandlerBase
    {
        // Error codes for Windows/Xbox (HRESULT) translated as not enough space on disk
        private const int HR_ERROR_DISK_FULL = unchecked((int)0x80070070);
        private const int HR_ERROR_HANDLE_DISK_FULL = unchecked((int)0x80070027);

        public override ESerializationOperationResult HandleException(Exception exception)
        {
            if (exception is DirectoryNotFoundException)
                return ESerializationOperationResult.Failure | ESerializationOperationResult.DirectoryNotFound;

            if (exception is FileNotFoundException)
                return ESerializationOperationResult.Failure | ESerializationOperationResult.FileNotFound;

            if (exception is UnauthorizedAccessException)
                return ESerializationOperationResult.Failure | ESerializationOperationResult.UnauthorizedAccess;

            if (exception is PathTooLongException)
                return ESerializationOperationResult.Failure | ESerializationOperationResult.PathTooLong;

            if (exception is CryptographicException)
                return ESerializationOperationResult.Failure | ESerializationOperationResult.CryptographicError;

            if (exception is IOException ioException)
            {
                if (ioException.HResult == HR_ERROR_DISK_FULL || ioException.HResult == HR_ERROR_HANDLE_DISK_FULL)
                    return ESerializationOperationResult.Failure | ESerializationOperationResult.NotEnoughSpace;
            }

            return ESerializationOperationResult.Failure | ESerializationOperationResult.Unknown;
        }
    }
}
