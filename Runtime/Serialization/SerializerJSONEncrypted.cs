using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using MA.Serialization.Serialization;

namespace MA.Serialization
{
    [CreateAssetMenu(fileName = "SerializerEncryptedJSON", menuName = "Toolbox/Serialization/Serializer/JSON Encrypted")]
    public class SerializerJSONEncrypted : SerializerBase
    {
        private const int StreamBufferSize = 4096;

        [SerializeField] private string fileExtension = ".sav";
        [SerializeField] private string filePattern = "*.sav";

        [SerializeField] private SerializationExceptionHandlerBase exceptionHandler;

        [Tooltip("Encryption key AES. Must be exactly 32 bytes long for AES-256.")]
        [SerializeField] private string encryptionKey;

        [Tooltip("Initialization vector (IV). Must be exactly 16 bytes long.")]
        [SerializeField] private string encryptionIV;

        private byte[] _cachedKey;
        private byte[] _cachedIV;

        private void OnEnable() => InitializeKeyArrays();

        private void OnValidate() => InitializeKeyArrays();

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
                byte[] plainBytes = Encoding.UTF8.GetBytes(toJson);

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                using (Aes aes = Aes.Create())
                {
                    aes.Key = _cachedKey;
                    aes.IV = _cachedIV;

                    using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                        cryptoStream.FlushFinalBlock();
                    }
                }

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
                byte[] plainBytes = Encoding.UTF8.GetBytes(toJson);

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, StreamBufferSize, useAsync: true))
                using (Aes aes = Aes.Create())
                {
                    aes.Key = _cachedKey;
                    aes.IV = _cachedIV;

                    using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        await cryptoStream.WriteAsync(plainBytes, 0, plainBytes.Length);
                    }
                }

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

                string json;

                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (Aes aes = Aes.Create())
                {
                    aes.Key = _cachedKey;
                    aes.IV = _cachedIV;

                    using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    using (StreamReader reader = new StreamReader(cryptoStream, Encoding.UTF8))
                    {
                        json = reader.ReadToEnd();
                    }
                }

                DataT data = JsonUtility.FromJson<DataT>(json);

                return (data, ESerializationOperationResult.Success);
            }
            catch (Exception ex)
            {
                ESerializationOperationResult result = exceptionHandler != null ? exceptionHandler.HandleException(ex) : ESerializationOperationResult.Failure | ESerializationOperationResult.Unknown;
                
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

                string json;

                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, StreamBufferSize, useAsync: true))
                using (Aes aes = Aes.Create())
                {
                    aes.Key = _cachedKey;
                    aes.IV = _cachedIV;

                    using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    using (StreamReader reader = new StreamReader(cryptoStream, Encoding.UTF8))
                    {
                        json = await reader.ReadToEndAsync();
                    }
                }

                await Awaitable.MainThreadAsync();

                DataT data = JsonUtility.FromJson<DataT>(json);

                return (data, ESerializationOperationResult.Success);
            }
            catch (Exception ex)
            {
                await Awaitable.MainThreadAsync();

                ESerializationOperationResult result = exceptionHandler != null ? exceptionHandler.HandleException(ex) : ESerializationOperationResult.Failure | ESerializationOperationResult.Unknown;
               
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

                foreach (string file in Directory.EnumerateFiles(folderPath, filePattern))
                {
                    string json;

                    using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                    using (Aes aes = Aes.Create())
                    {
                        aes.Key = _cachedKey;
                        aes.IV = _cachedIV;

                        using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                        using (StreamReader reader = new StreamReader(cryptoStream, Encoding.UTF8))
                        {
                            json = reader.ReadToEnd();
                        }
                    }

                    DataT data = JsonUtility.FromJson<DataT>(json);
                    dataList.Add(data);
                }

                return (dataList, ESerializationOperationResult.Success);
            }
            catch (Exception ex)
            {
                ESerializationOperationResult result = exceptionHandler != null ? exceptionHandler.HandleException(ex) : ESerializationOperationResult.Failure | ESerializationOperationResult.Unknown;
                
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

                string[] files = Directory.GetFiles(folderPath, filePattern);
                List<string> jsons = new List<string>(files.Length);

                foreach (string file in files)
                {
                    string json;

                    using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read, StreamBufferSize, useAsync: true))
                    using (Aes aes = Aes.Create())
                    {
                        aes.Key = _cachedKey;
                        aes.IV = _cachedIV;

                        using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                        using (StreamReader reader = new StreamReader(cryptoStream, Encoding.UTF8))
                        {
                            json = await reader.ReadToEndAsync();
                        }
                    }

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

                ESerializationOperationResult result = exceptionHandler != null ? exceptionHandler.HandleException(ex) : ESerializationOperationResult.Failure | ESerializationOperationResult.Unknown;
                
                return (dataList, result);
            }
        }

        private string CombineFilePath(string folderPath, string fileName) => Path.Combine(folderPath, string.Concat(fileName, fileExtension));

        private void InitializeKeyArrays()
        {
            if (string.IsNullOrEmpty(encryptionKey) || string.IsNullOrEmpty(encryptionIV))
                return;

            _cachedKey = Encoding.UTF8.GetBytes(encryptionKey);
            _cachedIV = Encoding.UTF8.GetBytes(encryptionIV);
        }

#if UNITY_EDITOR
        [ContextMenu("Generate Random Encryption Keys")]
        private void GenerateRandomKeys()
        {
            using (Aes aes = Aes.Create())
            {
                aes.GenerateKey();
                aes.GenerateIV();

                encryptionKey = Convert.ToBase64String(aes.Key).Substring(0, 32);
                encryptionIV = Convert.ToBase64String(aes.IV).Substring(0, 16);
            }
            InitializeKeyArrays();
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}
