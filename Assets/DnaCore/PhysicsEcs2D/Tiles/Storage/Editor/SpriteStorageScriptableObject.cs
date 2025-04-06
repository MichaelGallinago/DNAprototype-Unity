using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using AYellowpaper.SerializedCollections;
using DnaCore.PhysicsEcs2D.Tiles.Generators.Editor;
using DnaCore.Utilities;
using DnaCore.Utilities.Editor;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

namespace DnaCore.PhysicsEcs2D.Tiles.Storage.Editor
{
    [CreateAssetMenu(
        fileName = nameof(SpriteStorageScriptableObject),
        menuName = AssetMenuPaths.ScriptableObjects + nameof(SpriteStorageScriptableObject))]
    public class SpriteStorageScriptableObject : ScriptableObject
    {
        [SerializeField, SerializedDictionary(nameof(BitTile), nameof(SpriteStorageData))]
        private SerializedDictionary<BitTile, SpriteStorageData> _sprites;
        
        [SerializeField, SerializedDictionary(nameof(Sprite), nameof(BitTile))]
        private SerializedDictionary<Sprite, BitTile> _bitTiles;
        
        [field: SerializeField] public SpriteAtlas Atlas { get; private set; }
        
        [SerializeField] private FreeSpaceMap _freeSpaceMap;

        [SerializeField, HideInInspector] private StorageFolder _folder;
        
        private static readonly Rect SpriteRect = new(0f, 0f, TileConstants.Size, TileConstants.Size);
        private static readonly Vector2 SpritePivot = new(0.5f, 0.5f);
        private static readonly string[] FolderPathTransferArray = new string[1];
        private static readonly SpriteAtlas[] AtlasTransferArray = new SpriteAtlas[1];
        
        private readonly ushort[] _colorData = new ushort[TileConstants.PixelNumber];
        private readonly HashSet<(Sprite sprite, int index)> _spritesToSave = new();
        private readonly HashSet<Sprite> _spritesToRemove = new();

        private int MaxIndex => _sprites.Values.Select(sprite => sprite.Index).Prepend(-1).Max();

        private void OnEnable() => _folder.Init(this, "GeneratedSprites");
        
        public void Clear()
        {
            _spritesToSave.Clear();
            _spritesToRemove.Clear();
            _sprites.Clear();
            _freeSpaceMap.Clear();
            _bitTiles.Clear();
            
            AssetDatabaseUtilities.BeginTransaction(ClearFolder);
            EditorUtility.SetDirty(this);
            RepackAtlas();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetOrCreate(ref BitTile bitTile, out Sprite sprite)
        {
            if (_sprites.TryGetValue(bitTile, out SpriteStorageData data))
            {
                sprite = data.Sprite;
                return true;
            }
            
            sprite = Create(ref bitTile);
            return false;
        }
        
        public void AddToRemove(Sprite sprite) => _spritesToRemove.Add(sprite);

        public void SaveAssets()
        {
            AssetDatabaseUtilities.BeginTransaction(() =>
            {
                SaveNewSprites();
                DeleteUnusedSprites();
            });

            EditorUtility.SetDirty(this);
            RepackAtlas();
            _freeSpaceMap.Shrink(MaxIndex);
        }

        private void SaveNewSprites()
        {
            if (_spritesToSave.Count <= 0) return;
            foreach ((Sprite sprite, int index) in _spritesToSave)
            {
                var stringIndex = index.ToString();
                AssetDatabase.CreateAsset(sprite.texture, Path.Combine(_folder.Path, $"texture{stringIndex}.asset"));
                AssetDatabase.CreateAsset(sprite, Path.Combine(_folder.Path, $"sprite{stringIndex}.asset"));
                EditorUtility.SetDirty(sprite.texture);
                EditorUtility.SetDirty(sprite);
            }
            _spritesToSave.Clear();
        }

        private void DeleteUnusedSprites()
        {
            if (_spritesToRemove.Count <= 0) return;
            foreach (Sprite sprite in _spritesToRemove)
            {
                Remove(sprite);
            }
            _spritesToRemove.Clear();
            _freeSpaceMap.Shrink(MaxIndex);
        }

        private void Remove(Sprite sprite)
        {
            if (!_bitTiles.TryGetValue(sprite, out BitTile bitTile)) return;

            SpriteStorageData data = _sprites[bitTile];
            _sprites.Remove(bitTile);
            _bitTiles.Remove(sprite);
            _freeSpaceMap.Add(data.Index);
            
            Texture2D texture = sprite.texture;
            sprite.DeleteAsset();
            texture.DeleteAsset();
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

        private void RepackAtlas()
        {
            AtlasTransferArray[0] = Atlas;
            SpriteAtlasUtility.PackAtlases(AtlasTransferArray, BuildTarget.NoTarget);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Sprite Create(ref BitTile bitTile)
        {
            for (uint y = 0; y < TileConstants.Size; y++) 
            for (uint x = 0; x < TileConstants.Size; x++)
            {
                _colorData[y * TileConstants.Size + x] = bitTile[x, y] ? ushort.MaxValue : ushort.MinValue;
            }
            
            var sprite = Sprite.Create(CreateTexture(), SpriteRect, SpritePivot, 1f);
         
            int index = _freeSpaceMap.Take(_sprites.Count);
            _spritesToSave.Add((sprite, index));
            
            _sprites.Add(bitTile, new SpriteStorageData(index, sprite));
            _bitTiles.Add(sprite, bitTile);
            
            return sprite;
        }

        private Texture2D CreateTexture()
        {
            const int size = TileConstants.Size;
            var texture = new Texture2D(size, size, TextureFormat.ARGB4444, false)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Mirror
            };
            texture.SetPixelData(_colorData, 0);
            texture.Apply();
            return texture;
        }
    }
}
