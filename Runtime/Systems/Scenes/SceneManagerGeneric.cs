using System.Collections.Generic;
using UnityEngine;

namespace MA.Systems.Scenes
{
    public abstract class SceneManagerGeneric<SceneKeyT, SceneDataT, LevelKeyT, LevelDataT> : MonoBehaviour, ISceneManagerGeneric<SceneKeyT, SceneDataT, LevelKeyT, LevelDataT>
    {
        [SerializeField] protected AssetMapGeneric<SceneKeyT, SceneDataT> keyToSceneMap;
        [SerializeField] protected AssetMapGeneric<LevelKeyT, LevelDataT> keyToLevelMap;

        public abstract HashSet<SceneDataT> ActiveScenes { get; }

        public abstract HashSet<LevelDataT> ActiveLevels { get; }

        public async Awaitable<ESceneOperationResult> LoadSceneAsync(SceneKeyT key) 
        {
            if (keyToSceneMap == null)
                return ESceneOperationResult.Failure | ESceneOperationResult.MapNotAssigned;

            if (!keyToSceneMap.TryGetAsset(key, out SceneDataT data))
                return ESceneOperationResult.Failure | ESceneOperationResult.KeyNotFound;

            return await LoadSceneAsync(data);
        }

        public abstract Awaitable<ESceneOperationResult> LoadSceneAsync(SceneDataT loadableData);

        public async Awaitable<ESceneOperationResult> UnloadSceneAsync(SceneKeyT key) 
        {
            if (keyToSceneMap == null)
                return ESceneOperationResult.Failure | ESceneOperationResult.MapNotAssigned;

            if (!keyToSceneMap.TryGetAsset(key, out SceneDataT data))
                return ESceneOperationResult.Failure | ESceneOperationResult.KeyNotFound;

            return await UnloadSceneAsync(data);
        }

        public abstract Awaitable<ESceneOperationResult> UnloadSceneAsync(SceneDataT loadableData);

        public async Awaitable<ESceneOperationResult> LoadLevelAsync(LevelKeyT key) 
        {
            if (keyToLevelMap == null)
                return ESceneOperationResult.Failure | ESceneOperationResult.MapNotAssigned;

            if (!keyToLevelMap.TryGetAsset(key, out LevelDataT data))
                return ESceneOperationResult.Failure | ESceneOperationResult.KeyNotFound;

            return await LoadLevelAsync(data);
        }

        public abstract Awaitable<ESceneOperationResult> LoadLevelAsync(LevelDataT loadableData);

        public async Awaitable<ESceneOperationResult> UnloadLevelAsync(LevelKeyT key)
        {
            if (keyToLevelMap == null)
                return ESceneOperationResult.Failure | ESceneOperationResult.MapNotAssigned;

            if (!keyToLevelMap.TryGetAsset(key, out LevelDataT data))
                return ESceneOperationResult.Failure | ESceneOperationResult.KeyNotFound;

            return await UnloadLevelAsync(data);
        }

        public abstract Awaitable<ESceneOperationResult> UnloadLevelAsync(LevelDataT loadableData);
    }
}
