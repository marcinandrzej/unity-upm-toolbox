using System.Collections.Generic;
using UnityEngine;

namespace MA.Systems.Scenes
{
    public interface ISceneManagerGeneric<SceneKeyT, SceneDataT>
    {
        public IReadOnlyCollection<SceneDataT> ActiveSceneDataCollection { get; }

        public Awaitable<ESceneOperationResult> LoadAsync(SceneKeyT key);

        public Awaitable<ESceneOperationResult> LoadAsync(SceneDataT data);

        public Awaitable<ESceneOperationResult> UnloadAsync(SceneKeyT key);

        public Awaitable<ESceneOperationResult> UnloadAsync(SceneDataT data);

    }
}
