using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tiles.Storage.Editor
{
    [Serializable]
    public struct StorageFolder
    {
        [field: SerializeField, HideInInspector] public string StoragePath { get; private set; }
        [field: SerializeField, HideInInspector] public string Path { get; private set; }

        public void Init(Object storageAsset, string name)
        {
            StoragePath = System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(storageAsset));
            
            if (!string.IsNullOrEmpty(Path) && AssetDatabase.IsValidFolder(Path)) return;
            
            AssetDatabase.CreateFolder(StoragePath, name);
            Path = $"{StoragePath}\\{name}";
        }
    }
}
