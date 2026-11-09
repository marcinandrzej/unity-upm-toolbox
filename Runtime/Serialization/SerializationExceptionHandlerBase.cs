using System;
using UnityEngine;

namespace MA.Serialization.Serialization
{
    public abstract class SerializationExceptionHandlerBase : ScriptableObject
    {
        public abstract ESerializationOperationResult HandleException(Exception exception);
    }
}
