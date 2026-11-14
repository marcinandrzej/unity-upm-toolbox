using MA.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace MA.Systems.Scenes
{
    public abstract class SceneManagerGeneric<SceneKeyT, SceneDataT, SceneLoaderT> : MonoBehaviour, ISceneManagerGeneric<SceneKeyT, SceneDataT> where SceneLoaderT : ISceneLoaderGeneric<SceneDataT>, new()
    {
        [SerializeField] private AssetMapGeneric<SceneKeyT, SceneDataT> keyToDataMap;
        [SerializeField] private SceneLoaderT sceneLoader = new();

        public IReadOnlyCollection<SceneDataT> ActiveSceneDataCollection => sceneLoader.ActiveScenes;

        private void OnDestroy() => sceneLoader?.Dispose();

        public async Awaitable<ESceneOperationResult> LoadAsync(SceneKeyT key) 
        {
            if (keyToDataMap == null)
                return ESceneOperationResult.Failure | ESceneOperationResult.MapNotAssigned;

            if (!keyToDataMap.TryGetAsset(key, out SceneDataT data))
                return ESceneOperationResult.Failure | ESceneOperationResult.KeyNotFound;

            return await LoadAsync(data);
        }

        public Awaitable<ESceneOperationResult> LoadAsync(SceneDataT data) => sceneLoader.LoadAsync(data);

        public async Awaitable<ESceneOperationResult> UnloadAsync(SceneKeyT key) 
        {
            if (keyToDataMap == null)
                return ESceneOperationResult.Failure | ESceneOperationResult.MapNotAssigned;

            if (!keyToDataMap.TryGetAsset(key, out SceneDataT data))
                return ESceneOperationResult.Failure | ESceneOperationResult.KeyNotFound;

            return await UnloadAsync(data);
        }
        
        public  Awaitable<ESceneOperationResult> UnloadAsync(SceneDataT data) => sceneLoader.UnloadAsync(data);
    }
}
