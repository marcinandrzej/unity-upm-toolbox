using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MA.Systems.Scenes.Samples
{
    public class SampleSceneManager : SceneManagerGeneric<ESampleSceneKey, SampleSceneData, ESampleSceneKey, SampleSceneData>
    {
        private readonly HashSet<SampleSceneData> activeScenes = new();
        private readonly HashSet<SampleSceneData> activeLevels = new();

        public override HashSet<SampleSceneData> ActiveScenes => activeScenes;
        public override HashSet<SampleSceneData> ActiveLevels => activeLevels;

        public override async Awaitable<ESceneOperationResult> LoadSceneAsync(SampleSceneData loadableData)
        {
            if (loadableData == null || string.IsNullOrEmpty(loadableData.SceneName))
                return ESceneOperationResult.Failure | ESceneOperationResult.InvalidSceneData;

            if (activeScenes.Contains(loadableData))
                return ESceneOperationResult.Failure | ESceneOperationResult.SceneAlreadyLoaded;

            try
            {
                await SceneManager.LoadSceneAsync(loadableData.SceneName, LoadSceneMode.Additive);

                activeScenes.Add(loadableData);

                return ESceneOperationResult.Success;
            }
            catch
            {
                return ESceneOperationResult.Failure | ESceneOperationResult.Unknown;
            }
        }

        public override async Awaitable<ESceneOperationResult> UnloadSceneAsync(SampleSceneData loadableData)
        {
            if (loadableData == null || string.IsNullOrEmpty(loadableData.SceneName))
                return ESceneOperationResult.Failure | ESceneOperationResult.InvalidSceneData;

            if (!activeScenes.Contains(loadableData))
                return ESceneOperationResult.Failure | ESceneOperationResult.SceneNotLoaded;

            try
            {
                await SceneManager.UnloadSceneAsync(loadableData.SceneName);

                activeScenes.Remove(loadableData);

                return ESceneOperationResult.Success;
            }
            catch
            {
                return ESceneOperationResult.Failure | ESceneOperationResult.Unknown;
            }
        }

        public override async Awaitable<ESceneOperationResult> LoadLevelAsync(SampleSceneData loadableData)
        {
            if (loadableData == null || string.IsNullOrEmpty(loadableData.SceneName))
                return ESceneOperationResult.Failure | ESceneOperationResult.InvalidSceneData;

            if (activeLevels.Contains(loadableData))
                return ESceneOperationResult.Failure | ESceneOperationResult.SceneAlreadyLoaded;

            try
            {
                await SceneManager.LoadSceneAsync(loadableData.SceneName, LoadSceneMode.Additive);

                activeLevels.Add(loadableData);

                return ESceneOperationResult.Success;
            }
            catch
            {
                return ESceneOperationResult.Failure | ESceneOperationResult.Unknown;
            }
        }

        public override async Awaitable<ESceneOperationResult> UnloadLevelAsync(SampleSceneData loadableData)
        {
            if (loadableData == null || string.IsNullOrEmpty(loadableData.SceneName))
                return ESceneOperationResult.Failure | ESceneOperationResult.InvalidSceneData;

            if (!activeLevels.Contains(loadableData))
                return ESceneOperationResult.Failure | ESceneOperationResult.SceneNotLoaded;

            try
            {
                await SceneManager.UnloadSceneAsync(loadableData.SceneName);

                activeLevels.Remove(loadableData);

                return ESceneOperationResult.Success;
            }
            catch
            {
                return ESceneOperationResult.Failure | ESceneOperationResult.Unknown;
            }
        }
    }
}
