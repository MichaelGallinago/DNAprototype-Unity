using UnityEditor;
using UnityEngine;

namespace Utilities
{
    public static class UnityEngineObjectExtensions
    {
        public static void DeleteAsset(this Object assetObject) => 
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(assetObject));
    }
}
