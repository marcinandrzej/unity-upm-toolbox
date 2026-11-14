using System.Collections.Generic;
using UnityEngine;

namespace MA.Utilities
{
    public abstract class AssetMapGeneric<KeyT, AssetT> : ScriptableObject
    {
        protected abstract Dictionary<KeyT, AssetT> AssetMap { get; }

        public bool TryGetAsset(KeyT key, out AssetT asset) => AssetMap.TryGetValue(key, out asset);
    }
}
