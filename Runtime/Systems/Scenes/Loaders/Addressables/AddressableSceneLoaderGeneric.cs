#if ADDRESSABLES_ENABLED
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace MA.Systems.Scenes
{
    [Serializable]
    public class AddressableSceneLoaderGeneric<SceneDataT> : ISceneLoaderGeneric<SceneDataT> where SceneDataT : IAddressableSceneData
    {
        private readonly Dictionary<SceneDataT, AsyncOperationHandle<SceneInstance>> _dataToHandleDict = new();

        public IReadOnlyCollection<SceneDataT> ActiveScenes => _dataToHandleDict.Keys;

        public void Dispose()
        {
            foreach (AsyncOperationHandle<SceneInstance> handle in _dataToHandleDict.Values)
            {
                if (handle.IsValid())
                    Addressables.UnloadSceneAsync(handle, autoReleaseHandle: true);
            }

            _dataToHandleDict.Clear();
        }

        public async Awaitable<ESceneOperationResult> LoadAsync(SceneDataT data)
        {
            if (data == null || data.SceneReference == null || !data.SceneReference.RuntimeKeyIsValid())
                return ESceneOperationResult.Failure | ESceneOperationResult.InvalidSceneData;

            if (_dataToHandleDict.ContainsKey(data))
                return ESceneOperationResult.Failure | ESceneOperationResult.SceneAlreadyLoaded;

            try
            {
                AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(data.SceneReference, LoadSceneMode.Additive);

                _dataToHandleDict.Add(data, handle);

                await handle.Task;

                if (handle.Status != AsyncOperationStatus.Succeeded)
                {
                    _dataToHandleDict.Remove(data);

                    if (handle.IsValid())
                        Addressables.Release(handle);

                    return ESceneOperationResult.Failure | ESceneOperationResult.Unknown;
                }

                return ESceneOperationResult.Success;
            }
            catch
            {
                _dataToHandleDict.Remove(data);

                return ESceneOperationResult.Failure | ESceneOperationResult.Unknown;
            }
        }

        public async Awaitable<ESceneOperationResult> UnloadAsync(SceneDataT data)
        {
            if (data == null || data.SceneReference == null)
                return ESceneOperationResult.Failure | ESceneOperationResult.InvalidSceneData;

            if (!_dataToHandleDict.Remove(data, out AsyncOperationHandle<SceneInstance> handle))
                return ESceneOperationResult.Failure | ESceneOperationResult.SceneNotLoaded;

            if (!handle.IsValid())
                return ESceneOperationResult.Failure | ESceneOperationResult.Unknown;

            try
            {
                AsyncOperationHandle<SceneInstance> unloadHandle = Addressables.UnloadSceneAsync(handle, autoReleaseHandle: true);

                await unloadHandle.Task;

                return unloadHandle.Status == AsyncOperationStatus.Succeeded ? ESceneOperationResult.Success : ESceneOperationResult.Failure | ESceneOperationResult.Unknown;
            }
            catch
            {
                return ESceneOperationResult.Failure | ESceneOperationResult.Unknown;
            }
        }
    }
}
#endif