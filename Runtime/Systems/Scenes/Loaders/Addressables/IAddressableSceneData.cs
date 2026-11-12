#if ADDRESSABLES_ENABLED
using UnityEngine.AddressableAssets;

namespace MA.Systems.Scenes
{
    public interface IAddressableSceneData
    {
        public AssetReference SceneReference { get; }
    }
}
#endif