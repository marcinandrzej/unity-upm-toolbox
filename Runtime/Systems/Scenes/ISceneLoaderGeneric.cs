using System;
using System.Collections.Generic;
using UnityEngine;

namespace MA.Systems.Scenes
{
    public interface ISceneLoaderGeneric<SceneDataT> : IDisposable
    {
        public IReadOnlyCollection<SceneDataT> ActiveScenes { get; }

        public Awaitable<ESceneOperationResult> LoadAsync(SceneDataT data);

        public Awaitable<ESceneOperationResult> UnloadAsync(SceneDataT data);
    }
}
