using System;

namespace MA.Systems.Scenes
{
    [Flags]
    public enum ESceneOperationResult
    {
        None = 0,
        Success = 1 << 0,
        Failure = 1 << 1,
        Unknown = 1 << 2,
        MapNotAssigned = 1 << 3,
        KeyNotFound = 1 << 4,
        SceneAlreadyLoaded = 1 << 5,
        SceneNotLoaded = 1 << 6,
        InvalidSceneData = 1 << 7,
    }
}
