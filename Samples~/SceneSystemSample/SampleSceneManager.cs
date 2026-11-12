using UnityEngine;

namespace MA.Systems.Scenes.Samples
{
    public class SampleSceneManager : SceneManagerGeneric<ESampleSceneKey, SampleSceneData, BuildInSceneLoaderGeneric<SampleSceneData>>
    {
#if UNITY_EDITOR
        [Header("DEBUG")]
        public ESampleSceneKey debugSceneKey;
        public SampleSceneData debugSceneData;

        [ContextMenu("DebugUnloadSceneByKey")]
        public void DebugUnloadSceneByKey() => _ = UnloadAsync(debugSceneKey);

        [ContextMenu("DebugLoadSceneByKey")]
        public void DebugLoadSceneByKey() => _ = LoadAsync(debugSceneKey);

        [ContextMenu("DebugUnloadSceneByData")]
        public void DebugUnloadSceneByData() => _ = UnloadAsync(debugSceneData);

        [ContextMenu("DebugLoadSceneByData")]
        public void DebugLoadSceneByData() => _ = LoadAsync(debugSceneData);
#endif
    }
}
