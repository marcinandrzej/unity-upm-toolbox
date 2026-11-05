using System.Collections.Generic;
using UnityEngine;

namespace MA.Serialization
{
    public interface ISerializer
    {
        public bool FileExists(string folderPath, string fileName);

        public void DeleteFile(string folderPath, string fileName);

        public void SerializeDataToFile<DataT>(string folderPath, string fileName, DataT data);

        public Awaitable SerializeDataToFileAsync<DataT>(string folderPath, string fileName, DataT data);

        public DataT DeserializeDataFromFile<DataT>(string folderPath, string fileName);

        public Awaitable<DataT> DeserializeDataFromFileAsync<DataT>(string folderPath, string fileName);

        public List<DataT> DeserializeDataFromDirectory<DataT>(string folderPath);

        public Awaitable<List<DataT>> DeserializeDataFromDirectoryAsync<DataT>(string folderPath);
    }
}
