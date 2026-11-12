using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MA.Systems.Scenes
{
    [Serializable]
    public class BuildInSceneLoaderGeneric<SceneDataT> : ISceneLoaderGeneric<SceneDataT> where SceneDataT : IBuildInSceneData
    {
        private readonly HashSet<SceneDataT> _activeSceneDataSet = new();

        public IReadOnlyCollection<SceneDataT> ActiveScenes => _activeSceneDataSet;

        public void Dispose() => _activeSceneDataSet.Clear();

        public async Awaitable<ESceneOperationResult> LoadAsync(SceneDataT data)
        {
            if (data == null || string.IsNullOrEmpty(data.SceneName))
                return ESceneOperationResult.Failure | ESceneOperationResult.InvalidSceneData;

            if (!_activeSceneDataSet.Add(data))
                return ESceneOperationResult.Failure | ESceneOperationResult.SceneAlreadyLoaded;

            try
            {
                AsyncOperation asyncOp = SceneManager.LoadSceneAsync(data.SceneName, LoadSceneMode.Additive);

                if (asyncOp == null)
                {
                    _activeSceneDataSet.Remove(data);

                    return ESceneOperationResult.Failure | ESceneOperationResult.InvalidSceneData;
                }

                await asyncOp;

                return ESceneOperationResult.Success;
            }
            catch
            {
                _activeSceneDataSet.Remove(data);

                return ESceneOperationResult.Failure | ESceneOperationResult.Unknown;
            }
        }

        public async Awaitable<ESceneOperationResult> UnloadAsync(SceneDataT data)
        {
            if (data == null || string.IsNullOrEmpty(data.SceneName))
                return ESceneOperationResult.Failure | ESceneOperationResult.InvalidSceneData;

            if (!_activeSceneDataSet.Remove(data))
                return ESceneOperationResult.Failure | ESceneOperationResult.SceneNotLoaded;

            try
            {
                AsyncOperation asyncOp = SceneManager.UnloadSceneAsync(data.SceneName);

                if (asyncOp == null)
                    return ESceneOperationResult.Failure | ESceneOperationResult.SceneNotLoaded;

                await asyncOp;

                return ESceneOperationResult.Success;
            }
            catch
            {
                return ESceneOperationResult.Failure | ESceneOperationResult.Unknown;
            }
        }

    }
}
