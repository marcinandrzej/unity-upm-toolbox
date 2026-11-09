using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MA.Serialization.Serialization;

namespace MA.Serialization
{
    [CreateAssetMenu(fileName = "SerializerJSON", menuName = "Toolbox/Serialization/Serializer/JSON")]
    public class SerializerJSON : SerializerBase
    {
        private const string FileExtension = ".json";
        private const string FilePattern = "*.json";

        [SerializeField] private SerializationExceptionHandlerBase exceptionHandler;

        public override ESerializationOperationResult DeleteFile(string folderPath, string fileName)
        {
            string path = CombineFilePath(folderPath, fileName);

            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);

                    return ESerializationOperationResult.Success;
                }

                return ESerializationOperationResult.Failure | ESerializationOperationResult.FileNotFound;
            }
            catch (Exception ex)
            {
                return exceptionHandler != null ? exceptionHandler.HandleException(ex) : ESerializationOperationResult.Failure | ESerializationOperationResult.Unknown;
            }
        }

        public override ESerializationOperationResult SerializeDataToFile<DataT>(string folderPath, string fileName, DataT data)
        {
            string path = CombineFilePath(folderPath, fileName);

            try
            {
                string toJson = JsonUtility.ToJson(data);

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                File.WriteAllText(path, toJson);

                return ESerializationOperationResult.Success;
            }
            catch (Exception ex)
            {
                return exceptionHandler != null ? exceptionHandler.HandleException(ex) : ESerializationOperationResult.Failure | ESerializationOperationResult.Unknown;
            }
        }

        public override async Awaitable<ESerializationOperationResult> SerializeDataToFileAsync<DataT>(string folderPath, string fileName, DataT data)
        {
            string path = CombineFilePath(folderPath, fileName);

            try
            {
                string toJson = JsonUtility.ToJson(data);

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                await File.WriteAllTextAsync(path, toJson);

                await Awaitable.MainThreadAsync();

                return ESerializationOperationResult.Success;
            }
            catch (Exception ex)
            {
                await Awaitable.MainThreadAsync();

                return exceptionHandler != null ? exceptionHandler.HandleException(ex) : ESerializationOperationResult.Failure | ESerializationOperationResult.Unknown;
            }
        }

        public override (DataT data, ESerializationOperationResult result) DeserializeDataFromFile<DataT>(string folderPath, string fileName)
        {
            string path = CombineFilePath(folderPath, fileName);

            try
            {
                if (!File.Exists(path))
                    return (default, ESerializationOperationResult.Failure | ESerializationOperationResult.FileNotFound);

                string json = File.ReadAllText(path);
                DataT data = JsonUtility.FromJson<DataT>(json);

                return (data, ESerializationOperationResult.Success);
            }
            catch (Exception ex)
            {
                ESerializationOperationResult result = exceptionHandler != null ? exceptionHandler.HandleException(ex) : ESerializationOperationResult.Failure;

                return (default, result);
            }
        }

        public override async Awaitable<(DataT data, ESerializationOperationResult result)> DeserializeDataFromFileAsync<DataT>(string folderPath, string fileName)
        {
            string path = CombineFilePath(folderPath, fileName);

            try
            {
                if (!File.Exists(path))
                    return (default, ESerializationOperationResult.Failure | ESerializationOperationResult.FileNotFound);

                string json = await File.ReadAllTextAsync(path);

                await Awaitable.MainThreadAsync();

                DataT data = JsonUtility.FromJson<DataT>(json);

                return (data, ESerializationOperationResult.Success);
            }
            catch (Exception ex)
            {
                await Awaitable.MainThreadAsync();

                ESerializationOperationResult result = exceptionHandler != null ? exceptionHandler.HandleException(ex) : ESerializationOperationResult.Failure;
                
                return (default, result);
            }
        }

        public override (List<DataT> data, ESerializationOperationResult result) DeserializeDataFromDirectory<DataT>(string folderPath)
        {
            List<DataT> dataList = new List<DataT>();

            try
            {
                if (!Directory.Exists(folderPath))
                    return (dataList, ESerializationOperationResult.Failure | ESerializationOperationResult.DirectoryNotFound);

                IEnumerable<string> files = Directory.EnumerateFiles(folderPath, FilePattern);

                foreach (string file in files)
                {
                    string json = File.ReadAllText(file);
                    DataT data = JsonUtility.FromJson<DataT>(json);
                    dataList.Add(data);
                }

                return (dataList, ESerializationOperationResult.Success);
            }
            catch (Exception ex)
            {
                ESerializationOperationResult result = exceptionHandler != null ? exceptionHandler.HandleException(ex) : ESerializationOperationResult.Failure;
                
                return (dataList, result);
            }
        }

        public override async Awaitable<(List<DataT> data, ESerializationOperationResult result)> DeserializeDataFromDirectoryAsync<DataT>(string folderPath)
        {
            List<DataT> dataList = new List<DataT>();

            try
            {
                if (!Directory.Exists(folderPath))
                    return (dataList, ESerializationOperationResult.Failure | ESerializationOperationResult.DirectoryNotFound);

                string[] files = Directory.GetFiles(folderPath, FilePattern);
                List<string> jsons = new List<string>(files.Length);

                foreach (string file in files)
                {
                    string json = await File.ReadAllTextAsync(file);
                    jsons.Add(json);
                }

                await Awaitable.MainThreadAsync();

                foreach (string json in jsons)
                {
                    DataT data = JsonUtility.FromJson<DataT>(json);
                    dataList.Add(data);
                }

                return (dataList, ESerializationOperationResult.Success);
            }
            catch (Exception ex)
            {
                await Awaitable.MainThreadAsync();

                ESerializationOperationResult result = exceptionHandler != null ? exceptionHandler.HandleException(ex) : ESerializationOperationResult.Failure;
                
                return (dataList, result);
            }
        }

        private string CombineFilePath(string folderPath, string fileName) => Path.Combine(folderPath, string.Concat(fileName, FileExtension));
    }
}
