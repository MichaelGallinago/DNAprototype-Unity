using System;
using UnityEditor;
using UnityEngine;

namespace DnaCore.PhysicsEcs2D.Tiles.Storage.Editor
{
    [Serializable]
    public struct StorageFolder
    {
        [field: SerializeField, HideInInspector] public string StoragePath { get; private set; }
        [field: SerializeField, HideInInspector] public string Path { get; private set; }

        public void Init(UnityEngine.Object storageAsset, string name)
        {
            StoragePath = System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(storageAsset));

            Debug.Log($"{string.IsNullOrEmpty(Path).ToString()} : {AssetDatabase.IsValidFolder(Path)}");
            if (!string.IsNullOrEmpty(Path) && AssetDatabase.IsValidFolder(Path)) return;
            
            AssetDatabase.CreateFolder(StoragePath, name);
            Path = $"{StoragePath}\\{name}";
        }
    }
}
