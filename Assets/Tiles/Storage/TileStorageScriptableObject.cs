using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AYellowpaper.SerializedCollections;
using Tiles.Generator;
using Tiles.SolidTypes;
using UnityEditor;
using UnityEngine;
using Utilities;
using static Utilities.ScriptableObjectUtilities;

namespace Tiles.Storage
{
    [CreateAssetMenu(
        fileName = nameof(TileStorageScriptableObject),
        menuName = ScriptableObjectsFolder + nameof(TileStorageScriptableObject))]
    public class TileStorageScriptableObject : ScriptableObject
    {
        [SerializeField, SerializedDictionary(nameof(TileKey), nameof(TileStorageData))]
        private SerializedDictionary<TileKey, TileStorageData> _tiles;
        
        [SerializeField] private FreeSpaceMap _freeSpaceMap;
        
        [SerializeField] private SizeDataStorageScriptableObject _sizeDataStorage;
        [SerializeField] private SpriteStorageScriptableObject _spriteStorage;

        [SerializeField, HideInInspector] private StorageFolder _folder;
        
        private static readonly string[] FolderPathTransferArray = new string[1];
        
        private readonly List<(GeneratedTile tile, string index)> _tilesToSave = new();
        private readonly List<GeneratedTile> _tilesToRemove = new();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GeneratedTile CreateIfDifferent(ref BitTile bitTile, SolidType solidType)
        {
            bool isOldSprite = _spriteStorage.TryGetOrCreate(ref bitTile, out Sprite sprite);
            if (isOldSprite && TryGet(new TileKey(sprite, solidType), out GeneratedTile tile)) return tile;
            
            tile = Create(ref bitTile, solidType, sprite);
            int index = _freeSpaceMap.Take();
            index = index < 0 ? _tiles.Count : index;
            
            _tilesToSave.Add((tile, index.ToString()));
            _tiles.Add(new TileKey(tile.Sprite, solidType), new TileStorageData(1, index, tile));
            
            return tile;
        }

        public void SaveAssets()
        {
            AssetDatabaseUtilities.BeginTransaction(SaveNewTiles);
            AssetDatabaseUtilities.BeginTransaction(DeleteUnusedTiles);
            
            _spriteStorage.SaveAssets();
            
            AssetDatabaseUtilities.SetDirtyAndSave(this);
        }
        
        public void AddToRemove(GeneratedTile tile) => _tilesToRemove.Add(tile);

        private void SaveNewTiles()
        {
            if (_tilesToSave.Count <= 0) return;
            foreach ((GeneratedTile tile, string index) in _tilesToSave)
            {
                AssetDatabase.CreateAsset(tile, $"{_folder.Path}\\tile{index}.asset");
            }
            _tilesToSave.Clear();
        }
        
        private void DeleteUnusedTiles()
        {
            if (_tilesToRemove.Count <= 0) return;
            foreach (GeneratedTile tile in _tilesToRemove)
            {
                Remove(tile);
            }
            _tilesToRemove.Clear();
        }
        
        private void Remove(GeneratedTile tile)
        {
            var key = new TileKey(tile.Sprite, tile.SolidType);
            if (!_tiles.TryGetValue(key, out TileStorageData data)) return;
            
            if (data.Count > 1)
            {
                data.Count--;
                _tiles[key] = data;
                return;
            }
            
            _tiles.Remove(key);
            _freeSpaceMap.Add(data.Index);
            data.Tile.DeleteAsset();
            
            if (ContainsSprite(tile.Sprite)) return;
            _spriteStorage.AddToRemove(tile.Sprite);
        }
        
        public void Clear()
        {
            _spriteStorage.Clear();
            _sizeDataStorage.Clear();
            _tiles.Clear();
            
            AssetDatabaseUtilities.BeginTransaction(ClearFolder);
            AssetDatabaseUtilities.SetDirtyAndSave(this);
        }
        
        private void OnValidate() => _folder.Init(this, "GeneratedTiles");

        private void ClearFolder()
        {
            FolderPathTransferArray[0] = _folder.Path;
            string[] assetPaths = AssetDatabase.FindAssets(string.Empty, FolderPathTransferArray);
            foreach (string assetGuid in assetPaths)
            {
                AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(assetGuid));
            }
        }

        private bool ContainsSprite(Sprite sprite)
        {
            foreach (SolidType type in SolidTypeExtensions.Values)
            {
                if (_tiles.ContainsKey(new TileKey(sprite, type))) return true;
            }
            
            return false;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryGet(in TileKey key, out GeneratedTile tile)
        {
            if (!_tiles.ContainsKey(key))
            {
                tile = null;
                return false;
            }
            
            TileStorageData data = _tiles[key];
            data.Count++;
            _tiles[key] = data;
            tile = data.Tile;
            return true;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private GeneratedTile Create(ref BitTile bitTile, SolidType solidType, Sprite sprite)
        {
            bitTile.GetSizes(solidType, out BitTile.SizeDto sizes);
            
            SizeData down = _sizeDataStorage[sizes.Down];
            SizeData right = _sizeDataStorage[sizes.Right];
            SizeData up = _sizeDataStorage[sizes.Up];
            SizeData left = _sizeDataStorage[sizes.Left];
            
            return GeneratedTile.Create(
                solidType, sprite,
                new Vector4(down.Angle[0], right.Angle[1], up.Angle[2], left.Angle[3]),
                down.Array, right.Array, up.Array, left.Array
            );
        }
    }
}
