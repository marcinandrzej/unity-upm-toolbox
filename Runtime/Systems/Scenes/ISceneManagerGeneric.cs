using System.Collections.Generic;
using UnityEngine;

namespace MA.Systems.Scenes
{
    public interface ISceneManagerGeneric<SceneKeyT, SceneDataT, LevelKeyT, LevelDataT>
    {
        public HashSet<SceneDataT> ActiveScenes { get; }

        public HashSet<LevelDataT> ActiveLevels { get; }

        public Awaitable<ESceneOperationResult> LoadSceneAsync(SceneKeyT key);

        public Awaitable<ESceneOperationResult> LoadSceneAsync(SceneDataT loadableData);

        public Awaitable<ESceneOperationResult> UnloadSceneAsync(SceneKeyT key);

        public Awaitable<ESceneOperationResult> UnloadSceneAsync(SceneDataT loadableData);

        public Awaitable<ESceneOperationResult> LoadLevelAsync(LevelKeyT key);

        public Awaitable<ESceneOperationResult> LoadLevelAsync(LevelDataT loadableData);

        public Awaitable<ESceneOperationResult> UnloadLevelAsync(LevelKeyT key);

        public Awaitable<ESceneOperationResult> UnloadLevelAsync(LevelDataT loadableData);
    }
}
