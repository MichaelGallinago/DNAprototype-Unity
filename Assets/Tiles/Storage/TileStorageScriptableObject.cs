#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using AYellowpaper.SerializedCollections;
using Tiles.Generator;
using Tiles.SolidTypes;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;
using static Tiles.TileUtilities;
using static Utilities.ScriptableObjectUtilities;
using Object = UnityEngine.Object;

namespace Tiles.Storage
{
    [CreateAssetMenu(
        fileName = nameof(TileStorageScriptableObject),
        menuName = ScriptableObjectsFolder + nameof(TileStorageScriptableObject))]
    public class TileStorageScriptableObject : ScriptableObject
    {
        [SerializeField, SerializedDictionary(nameof(BitTile), nameof(TileStorageData))]
        private SerializedDictionary<TileKey, TileStorageData> _tiles;

        [SerializeField, SerializedDictionary(nameof(BitTile), nameof(TileStorageData))]
        private SerializedDictionary<BitTile, Sprite> _sprites;

        [field: SerializeReference] public SpriteAtlas Atlas { get; private set; }
        [SerializeReference] private SizeDataStorageScriptableObject _sizeDataStorage;

        [SerializeField] private List<int> _freeSpaceList;

        [SerializeField, HideInInspector] private string _path;
        [SerializeField, HideInInspector] private string _atlasPath;
        [SerializeField, HideInInspector] private string _folderPath;
        [SerializeReference, HideInInspector] private SpriteAtlasAsset _atlasAsset;

        private readonly byte[] _colorData = new byte[PixelNumber];
        private static readonly Object[] AtlasAssetTransferArray = new Object[1];
        private static readonly string[] FolderPathTransferArray = new string[1];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GeneratedTile AddOrReplace(ref BitTile bitTile, SolidType solidType)
        {
            bool isOldSprite = TryGetOrCreateSprite(ref bitTile, out Sprite sprite);
            if (isOldSprite && TryGetTile(new TileKey(sprite, solidType), out GeneratedTile tile)) return tile;
            
            int index = GetFreeTileIndex();
            var stringIndex = index.ToString();
            if (!isOldSprite)
            {
                AssetDatabase.CreateAsset(sprite.texture, $"{_folderPath}\\texture{stringIndex}.asset");
                AssetDatabase.CreateAsset(sprite, $"{_folderPath}\\sprite{stringIndex}.asset");
                _sprites.Add(bitTile, sprite);
                AtlasAssetTransferArray[0] = sprite;
                _atlasAsset.Add(AtlasAssetTransferArray);
                SpriteAtlasAsset.Save(_atlasAsset, _atlasPath);
            }
            
            tile = CreateTile(ref bitTile, solidType, sprite);
            AssetDatabase.CreateAsset(tile, $"{_folderPath}\\tile{stringIndex}.asset");
            _tiles.Add(new TileKey(tile.Sprite, solidType), new TileStorageData(1, index, tile));
            
            AssetDatabase.Refresh();
            return tile;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(GeneratedTile tile)
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
            _freeSpaceList.Add(data.Index);
            DeleteAsset(data.Tile);
            
            if (ContainsSprite(tile.Sprite)) return;
            RemoveSprite(tile.Sprite);
        }

        private bool ContainsSprite(Sprite sprite)
        {
            foreach (SolidType type in SolidTypeExtensions.Values)
            {
                if (_tiles.ContainsKey(new TileKey(sprite, type))) return true;
            }
            
            return false;
        }

        private void RemoveSprite(Sprite sprite)
        {
            foreach (KeyValuePair<BitTile, Sprite> pair in _sprites)
            {
                if (pair.Value != sprite) continue;
                _sprites.Remove(pair.Key);
                break;
            }
            
            AtlasAssetTransferArray[0] = sprite;
            _atlasAsset.Remove(AtlasAssetTransferArray);
            SpriteAtlasAsset.Save(_atlasAsset, _atlasPath);
            AssetDatabase.Refresh();
            
            DeleteAsset(sprite);
            DeleteAsset(sprite.texture);
        }

        public void Clear()
        {
            _atlasAsset.Remove(Atlas.GetPackables());
            SpriteAtlasAsset.Save(_atlasAsset, _atlasPath);
            AssetDatabase.Refresh();
            
            _sizeDataStorage.Clear();
            _tiles.Clear();

            FolderPathTransferArray[0] = _folderPath;
            string[] assetPaths = AssetDatabase.FindAssets(string.Empty, FolderPathTransferArray);
            foreach (string assetGuid in assetPaths)
            {
                AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(assetGuid));
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryGetTile(in TileKey key, out GeneratedTile tile)
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
        private bool TryGetOrCreateSprite(ref BitTile bitTile, out Sprite sprite)
        {
            if (!_sprites.ContainsKey(bitTile))
            {
                sprite = CreateSprite(ref bitTile);
                return false;
            }

            sprite = _sprites[bitTile];
            return true;
        }
        
        private static void DeleteAsset(Object asset) => AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(asset));
        
        private int GetFreeTileIndex()
        {
            if (_freeSpaceList.Count <= 0) return _tiles.Count;
            
            int index = _freeSpaceList.Count - 1;
            int tileIndex = _freeSpaceList[index];
            _freeSpaceList.RemoveAt(index);
            return tileIndex;
        }

        private void OnValidate() => InitAtlasAsset();

        private void Awake()
        {
            InitAtlasAsset();
            _path = Path.GetDirectoryName(AssetDatabase.GetAssetPath(this));
            
            if (!string.IsNullOrEmpty(_folderPath) && AssetDatabase.IsValidFolder(_folderPath)) return;
            
            AssetDatabase.CreateFolder(_path, "TileAssets");
            _folderPath = _path + "\\TileAssets";
        }

        private void InitAtlasAsset()
        {
            if (!Atlas) return;
            string path = AssetDatabase.GetAssetPath(Atlas);
            if (_atlasPath == path && _atlasAsset) return;

            _atlasAsset = SpriteAtlasAsset.Load(_atlasPath = path);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private GeneratedTile CreateTile(ref BitTile bitTile, SolidType solidType, Sprite sprite)
        {
            bitTile.GetSizes(out BitTile.SizeDto sizes);
            
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
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Sprite CreateSprite(ref BitTile bitTile)
        {
            for (uint y = 0; y < Size; y++)
            for (uint x = 0; x < Size; x++)
            {
                _colorData[y * Size + x] = bitTile[x, y] ? byte.MaxValue : byte.MinValue;
            }

            var texture = new Texture2D(Size, Size, TextureFormat.Alpha8, false)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Mirror
            };
            texture.SetPixelData(_colorData, 0);
            texture.Apply();
            
            return Sprite.Create(texture, new Rect(0, 0, 16, 16), new Vector2(0.5f, 0.5f));
        }
    }
}
#endif
