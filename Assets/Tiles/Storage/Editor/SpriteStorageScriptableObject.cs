using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AYellowpaper.SerializedCollections;
using Tiles.Generators.Editor;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;
using Utilities;

namespace Tiles.Storage.Editor
{
    [CreateAssetMenu(
        fileName = nameof(SpriteStorageScriptableObject),
        menuName = ScriptableObjectUtilities.Folder + nameof(SpriteStorageScriptableObject))]
    public class SpriteStorageScriptableObject : ScriptableObject
    {
        [SerializeField, SerializedDictionary(nameof(BitTile), nameof(SpriteStorageData))]
        private SerializedDictionary<BitTile, SpriteStorageData> _sprites;
        
        [SerializeField, SerializedDictionary(nameof(Sprite), nameof(BitTile))]
        private SerializedDictionary<Sprite, BitTile> _bitTiles;
        
        [field: SerializeField] public SpriteAtlas Atlas { get; private set; }
        
        [SerializeField] private FreeSpaceMap _freeSpaceMap;
        
        [SerializeField, HideInInspector] private string _atlasPath;
        [SerializeField, HideInInspector] private StorageFolder _folder;
        
        private static readonly Rect SpriteRect = new(0f, 0f, TileConstants.Size, TileConstants.Size);
        private static readonly Vector2 SpritePivot = new(0.5f, 0.5f);
        private static readonly string[] FolderPathTransferArray = new string[1];
        
        private readonly ushort[] _colorData = new ushort[TileConstants.PixelNumber];
        private readonly HashSet<(Sprite sprite, string index)> _spritesToSave = new();
        private readonly HashSet<Sprite> _spritesToRemove = new();
        
        private SpriteAtlasAsset _atlasAsset;
        
        private void OnEnable()
        {
            InitAtlasAsset();
            _folder.Init(this, "GeneratedSprites");
        }
        
        public void Clear()
        {
            RemoveFromAtlas(Atlas.GetPackables());
            
            _spritesToSave.Clear();
            _spritesToRemove.Clear();
            _sprites.Clear();
            _freeSpaceMap.Clear();
            _bitTiles.Clear();
            
            AssetDatabaseUtilities.BeginTransaction(ClearFolder);

            EditorUtility.SetDirty(this);
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
            AssetDatabaseUtilities.BeginTransaction(SaveNewSprites);
            
            FillAtlas();
            ClearAtlas();
            
            AssetDatabaseUtilities.BeginTransaction(DeleteUnusedSprites);
            
            EditorUtility.SetDirty(Atlas);
            EditorUtility.SetDirty(_atlasAsset);
            EditorUtility.SetDirty(this);
        }

        private void SaveNewSprites()
        {
            if (_spritesToSave.Count <= 0) return;
            foreach ((Sprite sprite, string index) in _spritesToSave)
            {
                AssetDatabase.CreateAsset(sprite.texture, $"{_folder.Path}\\texture{index}.asset");
                AssetDatabase.CreateAsset(sprite, $"{_folder.Path}\\sprite{index}.asset");
                EditorUtility.SetDirty(sprite.texture);
                EditorUtility.SetDirty(sprite);
            }
        }

        private void ClearAtlas()
        {
            var index = 0;
            var spritesToAtlasDeletion = new Object[_spritesToRemove.Count];
            foreach (Sprite sprite in _spritesToRemove)
            {
                spritesToAtlasDeletion[index++] = sprite;
            }
            
            RemoveFromAtlas(spritesToAtlasDeletion);
        }

        private void FillAtlas()
        {
            var index = 0;
            var spritesToAtlasInsertion = new Object[_spritesToSave.Count];
            foreach ((Sprite sprite, string _) in _spritesToSave)
            {
                spritesToAtlasInsertion[index++] = sprite;
            }
            
            _spritesToSave.Clear();
            _atlasAsset.Add(spritesToAtlasInsertion);
        }

        private void DeleteUnusedSprites()
        {
            if (_spritesToRemove.Count <= 0) return;
            foreach (Sprite sprite in _spritesToRemove)
            {
                Remove(sprite);
            }
            _spritesToRemove.Clear();
        }

        private void Remove(Sprite sprite)
        {
            if (!_bitTiles.TryGetValue(sprite, out BitTile bitTile)) return;
            
            _sprites.Remove(bitTile);
            _bitTiles.Remove(sprite);
            
            Texture2D texture = sprite.texture;
            sprite.DeleteAsset();
            texture.DeleteAsset();
        }

        private void InitAtlasAsset()
        {
            if (!Atlas) return;
            _atlasAsset = SpriteAtlasAsset.Load(_atlasPath = AssetDatabase.GetAssetPath(Atlas));
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

        private void RemoveFromAtlas(Object[] assets)
        {
            _atlasAsset.Remove(assets);
            SpriteAtlasAsset.Save(_atlasAsset, _atlasPath);
            AssetDatabaseUtilities.SetDirtyAndSave(_atlasAsset);
            AssetDatabaseUtilities.SetDirtyAndSave(Atlas);
            AssetDatabase.ImportAsset(_atlasPath);
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
            _spritesToSave.Add((sprite, index.ToString()));
            
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
