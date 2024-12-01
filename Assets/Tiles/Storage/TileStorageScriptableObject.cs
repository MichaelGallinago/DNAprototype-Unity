#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using AYellowpaper.SerializedCollections;
using Tiles.Generator;
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
        [SerializeField, SerializedDictionary("Bit tiles", "Sprites")]
        private SerializedDictionary<BitTile, TileStorageData> _tiles;
        
        [SerializeReference] private SpriteAtlas _atlas;
        [SerializeReference] private SizeDataStorageScriptableObject _sizeDataStorage;
        
        [SerializeField] private string _path;
        [SerializeField] private string _folderPath;
        
        [SerializeField] private List<int> _freeSpaceList;
        
        private readonly byte[] _colorData = new byte[PixelNumber];
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(ref BitTile bitTile)
        {
            if (_tiles.ContainsKey(bitTile))
            {
                TileStorageData data = _tiles[bitTile];
                data.Count++;
                _tiles[bitTile] = data;
            }

            int index = GetFreeTileIndex();
            GeneratedTile tile = CreateTile(ref bitTile);
            
            var stringIndex = index.ToString();
            AssetDatabase.CreateAsset(tile.Sprite.texture, $"{_folderPath}\\texture{stringIndex}.asset");
            AssetDatabase.CreateAsset(tile.Sprite, $"{_folderPath}\\sprite{stringIndex}.asset");
            AssetDatabase.CreateAsset(tile, $"{_folderPath}\\tile{stringIndex}.asset");
            
            _tiles.Add(bitTile, new TileStorageData(1, index, tile));
            _atlas.Add(new Object[] {tile.Sprite});
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(ref BitTile bitTile)
        {
            if (!_tiles.ContainsKey(bitTile)) return;
            
            TileStorageData data = _tiles[bitTile];
            if (data.Count > 1)
            {
                data.Count--;
                _tiles[bitTile] = data;
                return;
            }
            
            _tiles.Remove(bitTile);
            _freeSpaceList.Add(data.Index);
            _atlas.Remove(new Object[] {data.Tile.Sprite});
            DeleteAsset(data.Tile);
            DeleteAsset(data.Tile.Sprite);
            DeleteAsset(data.Tile.Sprite.texture);
        }

        public void Clear()
        {
            _atlas.Remove(_atlas.GetPackables());
            _sizeDataStorage.Clear();
            _tiles.Clear();
            
            string[] assetPaths = AssetDatabase.FindAssets(string.Empty, new[] { _folderPath });
            foreach (string assetGuid in assetPaths)
            {
                AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(assetGuid));
            }
            
            AssetDatabase.Refresh();
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

        private void Awake()
        {
            _path = Path.GetDirectoryName(AssetDatabase.GetAssetPath(this));
            
            if (!string.IsNullOrEmpty(_folderPath) && AssetDatabase.IsValidFolder(_folderPath)) return;
            
            AssetDatabase.CreateFolder(_path, "TileAssets");
            _folderPath = _path + "\\TileAssets";
        }

        private GeneratedTile CreateTile(ref BitTile bitTile)
        {
            bitTile.GetSizes(out BitTile.SizeDto sizes);
            
            SizeData down = _sizeDataStorage[sizes.Down];
            SizeData right = _sizeDataStorage[sizes.Right];
            SizeData up = _sizeDataStorage[sizes.Up];
            SizeData left = _sizeDataStorage[sizes.Left];
            
            return GeneratedTile.Create(
                CreateSprite(ref bitTile), 
                down.Array, right.Array, up.Array, left.Array,
                new Vector4(down.Angle[0], right.Angle[1], up.Angle[2], left.Angle[3])
            );
        }
        
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
