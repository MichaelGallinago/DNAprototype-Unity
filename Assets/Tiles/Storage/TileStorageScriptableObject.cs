using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using AYellowpaper.SerializedCollections;
using BlobHashMaps;
using Tiles.Collision;
using Tiles.Generators;
using Tiles.Models;
using Tiles.SolidTypes;
using Unity.Collections;
using Unity.Entities;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utilities;
using static Utilities.ScriptableObjectUtilities;

namespace Tiles.Storage
{
    [CreateAssetMenu(
        fileName = nameof(TileStorageScriptableObject),
        menuName = ScriptableObjectsFolder + nameof(TileStorageScriptableObject))]
    public class TileStorageScriptableObject : ScriptableObject
    {
        private static readonly string[] FolderPathTransferArray = new string[1];
        
        [SerializeField, SerializedDictionary(nameof(TileKey), nameof(GeneratedTile))]
        private SerializedDictionary<TileKey, GeneratedTile> _tiles;
        
        [SerializeField] private FreeSpaceMap _freeSpaceMap;
        
        [SerializeField] private SizeDataStorageScriptableObject _sizeDataStorage;
        [SerializeField] private SpriteStorageScriptableObject _spriteStorage;

        [SerializeField, HideInInspector] private StorageFolder _folder;
        
        private readonly List<(GeneratedTile tile, string index)> _tilesToSave = new();
        private readonly List<GeneratedTile> _tilesToRemove = new();
        
        public static string BlobPath => Path.Combine(Application.streamingAssetsPath, "Blobs", "TileStorage.blob");

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GeneratedTile CreateIfDifferent(ref BitTile bitTile, SolidType solidType)
        {
            bool isOldSprite = _spriteStorage.TryGetOrCreate(ref bitTile, out Sprite sprite);
            if (isOldSprite && TryGet(new TileKey(sprite, solidType), out GeneratedTile tile)) return tile;

            int index = _freeSpaceMap.Take();
            index = index < 0 ? _tiles.Count : index;
            
            tile = Create(ref bitTile, solidType, sprite, index);
            _tilesToSave.Add((tile, index.ToString()));
            _tiles.Add(new TileKey(tile.Sprite, solidType), tile);
            
            return tile;
        }

        public void SaveAssets()
        {
            AssetDatabaseUtilities.BeginTransaction(SaveNewTiles);
            AssetDatabaseUtilities.BeginTransaction(DeleteUnusedTiles);
            
            _spriteStorage.SaveAssets();
            SaveBlobData();

            EditorUtility.SetDirty(this);
        }
        
        public void AddToRemove(GeneratedTile tile) => _tilesToRemove.Add(tile);
        
        public void DeleteTilesFromTilemap(TileBase[] tiles)
        {
            if (tiles.Length <= 0) return;
            foreach (TileBase tileBase in tiles)
            {
                if (tileBase is not GeneratedTile generatedTile) continue;
                Remove(generatedTile);
            }
        }
        
        private void SaveBlobData()
        {
            var builder = new BlobBuilder(Allocator.Temp);
            var hashMap = new NativeParallelHashMap<int, TileBlob>(_tiles.Count, Allocator.Temp);
            foreach (GeneratedTile data in _tiles.Values)
            {
                if (hashMap.ContainsKey(data.Index)) continue;
                
                ref TileBlob tile = ref builder.ConstructRoot<TileBlob>();

                tile.HeightsDown.Fill(builder, data.CollisionData.HeightsDown);
                tile.WidthsRight.Fill(builder, data.CollisionData.WidthsRight);
                tile.HeightsUp.Fill(builder, data.CollisionData.HeightsUp);
                tile.WidthsLeft.Fill(builder, data.CollisionData.WidthsLeft);
                tile.Angles = data.CollisionData.Angles;
                
                hashMap.Add(data.Index, tile);
            }
            
            builder.ConstructHashMap(ref builder.ConstructRoot<BlobHashMap<int, TileBlob>>(), ref hashMap);
            hashMap.Dispose();
            
            BlobAssetReference<BlobArray<TileBlob>>.Write(builder, BlobPath, 0);
            builder.Dispose();
        }
        
        private void SaveNewTiles()
        {
            if (_tilesToSave.Count <= 0) return;
            foreach ((GeneratedTile tile, string index) in _tilesToSave)
            {
                AssetDatabase.CreateAsset(tile, $"{_folder.Path}\\tile{index}.asset");
                EditorUtility.SetDirty(tile);
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
            
            if (tile.Count > 1)
            {
                tile.Count--;
                _tiles[key] = tile;
                return;
            }
            
            _tiles.Remove(key);
            _freeSpaceMap.Add(tile.Index);
            
            TileCollisionData collisionData = tile.CollisionData;
            _sizeDataStorage.Remove(collisionData.HeightsDown);
            _sizeDataStorage.Remove(collisionData.WidthsRight);
            _sizeDataStorage.Remove(collisionData.HeightsUp);
            _sizeDataStorage.Remove(collisionData.WidthsLeft);
            
            Sprite sprite = tile.Sprite;
            tile.DeleteAsset();
            
            if (ContainsSprite(sprite)) return;
            _spriteStorage.AddToRemove(sprite);
        }
        
        public void Clear()
        {
            _spriteStorage.Clear();
            _sizeDataStorage.Clear();
            _freeSpaceMap.Clear();
            _tiles.Clear();
            _tilesToSave.Clear();
            _tilesToRemove.Clear();
            
            AssetDatabaseUtilities.BeginTransaction(ClearFolder);
            
            EditorUtility.SetDirty(this);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
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
            
            tile = _tiles[key];
            tile.Count++;
            _tiles[key] = tile;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private GeneratedTile Create(ref BitTile bitTile, SolidType solidType, Sprite sprite, int index)
        {
            bitTile.GetSizes(solidType, out BitTile.SizeDto sizes);

            SizeData down = _sizeDataStorage[sizes.Down];
            SizeData right = _sizeDataStorage[sizes.Right];
            SizeData up = _sizeDataStorage[sizes.Up];
            SizeData left = _sizeDataStorage[sizes.Left];

            return GeneratedTile.Create(solidType, sprite, index, new TileCollisionData(
                down.Array, right.Array, up.Array, left.Array, 
                new Vector4(down.Angle[0], right.Angle[1], up.Angle[2], left.Angle[3]))
            );
        }
    }
}
