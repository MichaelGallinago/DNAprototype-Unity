using System;
using System.Runtime.CompilerServices;
using UnityEditor;

namespace Utilities
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

    }
}