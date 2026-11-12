using UnityEngine;

namespace MA.Systems.Scenes.Samples
{
    [CreateAssetMenu(fileName = "SampleSceneData", menuName = "Toolbox/Scenes/Samples/Scene Data")]
    public class SampleSceneData : ScriptableObject, IBuildInSceneData
    {
        [SerializeField] private string sceneName;

        public string SceneName => sceneName;
    }
}
