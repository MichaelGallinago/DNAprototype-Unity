using System;
using System.Runtime.CompilerServices;
using Tiles.Generator;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;
using Utilities;

namespace Tiles.Editor
{
    [CustomEditor(typeof(TileShape))]
    public class TileShapeEditor : UnityEditor.Editor
    {
        private static readonly int SolidTypeNumber = Enum.GetValues(typeof(GeneratedTile.SolidType)).Length;
        private static readonly Color32 White = new Color(255, 255, 255, 255);
        private static readonly Color32 Clear = new Color();
        
        private TileShape _tileShape;
        private Vector2Int _cellSize;
        private Texture2D _texture;
        private Color32[] _colorData;
        private RectInt _rect;
        private SpriteShapeRenderer _renderer;
        
        private void Awake()
        {
            _tileShape = (TileShape)target;
            
            Tilemap tileMap = _tileShape.TileMap;
            if (!tileMap) return;
            
            Vector3 cellSize = tileMap.cellSize;
            _cellSize = new Vector2Int((int)cellSize.x, (int)cellSize.y);
            _texture = new Texture2D(_cellSize.x, _cellSize.y);
            _colorData = new Color32[_cellSize.x * _cellSize.y];
            _renderer = _tileShape.Controller.spriteShapeRenderer;
            _rect = GetCeilRect(_renderer.bounds);
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            
            base.OnInspectorGUI();

            if (EditorGUI.EndChangeCheck())
            {
                _renderer.color = _tileShape.SolidType.GetColor();
            }
        }

        private void OnShapeChanged()
        {
            UpdateTilesInRect(_rect);
            RectInt rect = GetCeilRect(_renderer.bounds);
            if (rect == _rect) return;
            UpdateTilesInRect(_rect = rect);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private RectInt GetCeilRect(Bounds bounds)
        {
            var min = _tileShape.TileMap.WorldToCell(bounds.min).ToVector2Int();
            var max = _tileShape.TileMap.WorldToCell(bounds.max).ToVector2Int();
            return new RectInt(min, max - min);
        }
        
        private void UpdateTilesInRect(RectInt rect)
        {
            for (int ceilX = rect.x; ceilX <= rect.xMax; ceilX++)
            for (int ceilY = rect.y; ceilY <= rect.yMax; ceilY++)
            {
                GenerateTile(ceilX, ceilY);
            }
        }

        private void GenerateTile(int ceilX, int ceilY)
        {
            Vector2 worldPosition = new Vector2Int(ceilX, ceilY) * _cellSize + new Vector2(0.5f, 0.5f);
            var tile = new BitTile();
            
            Span<int> typeCounters = stackalloc int[SolidTypeNumber];
            
            for (var y = 0; y <= _cellSize.y; y++) 
            for (var x = 0; x <= _cellSize.x; x++)
            {
                Collider2D collider2d = Physics2D.OverlapPoint(worldPosition + new Vector2(x, y));
                
                if (collider2d)
                {
                    tile[x, y] = true;
                }

                typeCounters[(int)collider2d.GetComponent<TileShape>().SolidType]++;
            }
            
            GeneratedTile.SolidType type = GetFrequentSolidType(typeCounters);
            
            var tilePosition = new Vector3Int(ceilX, ceilY);
            var currentTile = _tileShape.TileMap.GetTile<GeneratedTile>(tilePosition);
            //_tileShape.TileMap.SetTile(tilePosition, _tile);
        }

        private static GeneratedTile.SolidType GetFrequentSolidType(Span<int> typeCounters)
        {
            var maxIndex = 0;
            int maxValue = typeCounters[0];

            for (var i = 1; i < typeCounters.Length; i++)
            {
                if (typeCounters[i] <= maxValue) continue;
                maxValue = typeCounters[i];
                maxIndex = i;
            }

            return (GeneratedTile.SolidType)maxIndex;
        }
        
        private Sprite CreateSprite(BitTile bitTile)
        {
            for (var y = 0; y < _cellSize.y; y++)
            for (var x = 0; x < _cellSize.x; x++)
            {
                _colorData[y * _cellSize.x + x] = bitTile[x, y] ? White : Clear;
            }
            _texture.SetPixels32(0, 0, _cellSize.x, _cellSize.y, _colorData);
            _texture.Apply();
            
            AssetDatabase.CreateAsset(_texture, "Assets/" + _texture.name + ".png");
            //File.WriteAllBytes(filePath, _texture.EncodeToPNG());

            var rect = new Rect(0, 0, _cellSize.x, _cellSize.y);
            var sprite = Sprite.Create(_texture, rect, new Vector2(0.5f, 0.5f));
            
            return sprite;
        }
    }
}