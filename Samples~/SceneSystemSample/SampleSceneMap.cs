using MA.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace MA.Systems.Scenes.Samples
{
    [CreateAssetMenu(fileName = "SampleSceneMap", menuName = "Toolbox/Scenes/Samples/Scene Map")]
    public class SampleSceneMap : AssetMapGeneric<ESampleSceneKey, SampleSceneData>
    {
        [SerializeField] private Dictionary<ESampleSceneKey, SampleSceneData> assetMap = new Dictionary<ESampleSceneKey, SampleSceneData>();

        protected override Dictionary<ESampleSceneKey, SampleSceneData> AssetMap => assetMap;
    }
}
