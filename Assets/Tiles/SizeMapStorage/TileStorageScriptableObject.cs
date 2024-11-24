#if UNITY_EDITOR
using System.Runtime.CompilerServices;
using AYellowpaper.SerializedCollections;
using Tiles.Generator;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;
using static Tiles.TileUtilities;
using static Utilities.ScriptableObjectUtilities;

namespace Tiles.SizeMapStorage
{
    [CreateAssetMenu(
        fileName = nameof(TileStorageScriptableObject), 
        menuName = ScriptableObjectsFolder + nameof(TileStorageScriptableObject))]
    public class TileStorageScriptableObject : ScriptableObject
    {
        private static readonly Color32 White = new(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        private static readonly Color32 Clear = new();
        
        [SerializeField, SerializedDictionary("Bit tiles", "Sprites")]
        private SerializedDictionary<BitTile, (int, GeneratedTile)> _tiles;
        
        [SerializeReference] private SpriteAtlas _atlas;
        [SerializeReference] private SizeMapStorageScriptableObject _sizeMaps;

        private Texture2D _texture;
        private string _path;

        private void Awake()
        {
            _path = AssetDatabase.GetAssetPath(this);
        }

        private void OnEnable()
        {
            _texture = new Texture2D(Size, Size, TextureFormat.ARGB32, false);
        }

        private readonly Color32[] _colorData = new Color32[PixelNumber];
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(ref BitTile key)
        {
            if (_tiles.ContainsKey(key))
            {
                (int Count, GeneratedTile) data = _tiles[key];
                data.Count++;
                _tiles[key] = data;
            }

            GeneratedTile tile = CreateTile(ref key);
            _tiles.Add(key, (1, tile));
            _atlas.Add(new Object[] {tile.Sprite});
            //AssetDatabase.CreateAsset(tile.Sprite.texture, tile.Sprite.texture);
            //AssetDatabase.CreateAsset(tile, tile);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(ref BitTile key)
        {
            if (!_tiles.ContainsKey(key)) return;
            
            (int Count, GeneratedTile Tile) data = _tiles[key];
            if (data.Count > 1)
            {
                data.Count--;
                _tiles[key] = data;
                return;
            }
            
            _tiles.Remove(key);
            _atlas.Remove(new Object[] {data.Tile.Sprite});
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(data.Tile.Sprite.texture));
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(data.Tile));
        }

        private GeneratedTile CreateTile(ref BitTile bitTile)
        {
            bitTile.GetSizes(out BitTile.SizeDto sizes);
            
            var angle = new Vector4(); //TODO: angles
            
            return GeneratedTile.Create(
                CreateSprite(ref bitTile), 
                _sizeMaps[sizes.Top], 
                _sizeMaps[sizes.Bottom], 
                _sizeMaps[sizes.Left], 
                _sizeMaps[sizes.Right], 
                angle);
        }
        
        private Sprite CreateSprite(ref BitTile bitTile)
        {
            for (var y = 0; y < Size; y++)
            for (var x = 0; x < Size; x++)
            {
                _colorData[y * Size + x] = bitTile[x, y] ? White : Clear;
            }
            _texture.SetPixels32(0, 0, Size, Size, _colorData);
            _texture.Apply();

            AssetDatabase.CreateAsset(_texture, "Assets/" + _atlas.name + ".png");
            //File.WriteAllBytes(filePath, _texture.EncodeToPNG());

            var rect = new Rect(0, 0, Size, Size);
            var sprite = Sprite.Create(_texture, rect, new Vector2(0.5f, 0.5f));
            
            return null;
        }
    }
}
#endif
