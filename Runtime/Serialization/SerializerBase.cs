using System.Collections.Generic;
using UnityEngine;

namespace MA.Serialization
{
    public abstract class SerializerBase : ScriptableObject
    {
        public abstract ESerializationOperationResult DeleteFile(string folderPath, string fileName);

        public abstract ESerializationOperationResult SerializeDataToFile<DataT>(string folderPath, string fileName, DataT data);

        public abstract Awaitable<ESerializationOperationResult> SerializeDataToFileAsync<DataT>(string folderPath, string fileName, DataT data);

        public abstract (DataT data, ESerializationOperationResult result) DeserializeDataFromFile<DataT>(string folderPath, string fileName);

        public abstract Awaitable<(DataT data, ESerializationOperationResult result)> DeserializeDataFromFileAsync<DataT>(string folderPath, string fileName);

        public abstract (List<DataT> data, ESerializationOperationResult result) DeserializeDataFromDirectory<DataT>(string folderPath);

        public abstract Awaitable<(List<DataT> data, ESerializationOperationResult result)> DeserializeDataFromDirectoryAsync<DataT>(string folderPath);
    }
}
