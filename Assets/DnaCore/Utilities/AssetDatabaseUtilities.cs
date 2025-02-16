#if UNITY_EDITOR
using System;
using System.Runtime.CompilerServices;
using UnityEditor;

namespace DnaCore.Utilities
{
    public static class AssetDatabaseUtilities
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BeginTransaction(Action action)
        {
            try
            {
                AssetDatabase.DisallowAutoRefresh();
                AssetDatabase.StartAssetEditing();

                action();
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
                AssetDatabase.AllowAutoRefresh();
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetDirtyAndSave(UnityEngine.Object asset)
        {
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssetIfDirty(asset);
        }
        
        public static void DeleteAsset(this UnityEngine.Object assetObject) => 
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(assetObject));
    }
}
#endif
