using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using AYellowpaper.SerializedCollections;
using DnaCore.PhysicsEcs2D.Tiles.Collision;
using DnaCore.PhysicsEcs2D.Tiles.Generators;
using DnaCore.PhysicsEcs2D.Tiles.Generators.Editor;
using DnaCore.PhysicsEcs2D.Tiles.SolidTypes;
using DnaCore.Utilities.Ecs;
using DnaCore.Utilities.Editor;
using Unity.Collections;
using Unity.Entities;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

using static DnaCore.Utilities.AssetMenuPaths;

namespace DnaCore.PhysicsEcs2D.Tiles.Storage.Editor
{
    [CreateAssetMenu(
        fileName = nameof(TileStorageScriptableObject),
        menuName = ScriptableObjects + nameof(TileStorageScriptableObject))]
    public class TileStorageScriptableObject : ScriptableObject
    {
        private static readonly string[] FolderPathTransferArray = new string[1];
        
        [SerializeField, SerializedDictionary(nameof(TileKey), nameof(GeneratedTile))]
        private SerializedDictionary<TileKey, GeneratedTile> _tiles;
        
        [SerializeField] private FreeSpaceMap _freeSpaceMap;
        
        [SerializeField] private SizeDataStorageScriptableObject _sizeDataStorage;
        [SerializeField] private SpriteStorageScriptableObject _spriteStorage;

        [SerializeField, HideInInspector] private StorageFolder _folder;
        
        private readonly List<GeneratedTile> _tilesToSave = new();
        private readonly List<GeneratedTile> _tilesToRemove = new();
        
        private int MaxIndex => _tiles.Values.Select(tile => tile.Index).Prepend(-1).Max();
        
        private void OnEnable() => _folder.Init(this, "GeneratedTiles");

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GeneratedTile CreateIfDifferent(ref BitTile bitTile, SolidType solidType)
        {
            bool isOldSprite = _spriteStorage.TryGetOrCreate(ref bitTile, out Sprite sprite);
            if (isOldSprite && TryGet(new TileKey(sprite, solidType), out GeneratedTile tile)) return tile;

            int index = _freeSpaceMap.Take(_tiles.Count);
            
            tile = Create(ref bitTile, solidType, sprite, index);
            _tilesToSave.Add(tile);
            _tiles.Add(new TileKey(tile.Sprite, solidType), tile);
            
            return tile;
        }

        public void SaveAssets()
        {
            AssetDatabaseUtilities.BeginTransaction(() =>
            {
                SaveNewTiles();
                DeleteUnusedTiles();
            });

            _spriteStorage.SaveAssets();
            SaveBlobData();

            EditorUtility.SetDirty(this);
            _freeSpaceMap.Shrink(MaxIndex);
        }
        
        public void AddToRemove(GeneratedTile tile) => _tilesToRemove.Add(tile);
        
        public void RemoveTiles(TileBase[] tiles) {
            foreach (TileBase tileBase in tiles) {
                if (tileBase is not GeneratedTile generatedTile) continue;
                Remove(generatedTile);
            }
        }

        private void SaveBlobData()
        {
            var builder = new BlobBuilder(Allocator.Temp);
            ref TilesBlob root = ref builder.ConstructRoot<TilesBlob>();
            
            BlobBuilderArray<NativeTile> arrayBuilder = builder.Allocate(ref root.Tiles, MaxIndex + 1);
            
            foreach (GeneratedTile data in _tiles.Values)
            {
                ref NativeTile tileRoot = ref arrayBuilder[data.Index];
                
                builder.Fill(ref tileRoot.HeightsDown, data.CollisionData.HeightsDown);
                builder.Fill(ref tileRoot.WidthsRight, data.CollisionData.WidthsRight);
                builder.Fill(ref tileRoot.HeightsUp, data.CollisionData.HeightsUp);
                builder.Fill(ref tileRoot.WidthsLeft, data.CollisionData.WidthsLeft);
                tileRoot.Angles = data.CollisionData.Angles;
            }
            
            BlobAssetReference<TilesBlob>.Write(builder, TileConstants.BlobPath, 0);
            builder.Dispose();
        }
        
        private void SaveNewTiles()
        {
            if (_tilesToSave.Count <= 0) return;
            foreach (GeneratedTile tile in _tilesToSave)
            {
                AssetDatabase.CreateAsset(tile, $"{_folder.Path}\\tile{tile.Index.ToString()}.asset");
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
            _freeSpaceMap.Shrink(MaxIndex);
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
