using System;
using System.Collections.Generic;
using Tiles.Generator;
using Tiles.SolidTypes;
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
        private readonly List<Collider2D> _colliders = new();
        
        private TileShape _tileShape;
        private Vector2Int _cellSize;
        private RectInt _rect;
        private ContactFilter2D _contactFilter;

        private SpriteShapeRenderer Renderer => _tileShape.Controller.spriteShapeRenderer;
        
        private void Awake()
        {
            _tileShape = (TileShape)target;

            Tilemap tileMap = _tileShape.TileMap;
            if (!tileMap) return;
            
            Vector3 cellSize = tileMap.cellSize;
            _cellSize = new Vector2Int((int)cellSize.x, (int)cellSize.y);
            _rect = GetCeilRect(Renderer.bounds);
            UpdateColor();
            
            _contactFilter.SetLayerMask(_tileShape.gameObject.layer);
        }
        
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            base.OnInspectorGUI();
            if (!EditorGUI.EndChangeCheck()) return;
            
            UpdateColor();
            UpdateMaterial();
            OnTransformChanged();
        }

        private void OnTransformChanged()
        {
            if (!_tileShape.transform.hasChanged) return;
            
            OnShapeChanged();
            _tileShape.transform.hasChanged = false;
        }

        private void UpdateColor() => Renderer.color = _tileShape.SolidType.GetColor();
        
        private void UpdateMaterial()
        {
            _tileShape.Collider.sharedMaterial = _tileShape.SolidTypes[_tileShape.SolidType];
        }
        
        private void OnShapeChanged()
        {
            if (!_tileShape.TileMap) return;
            
            UpdateTilesInRect(_rect);
            
            RectInt rect = GetCeilRect(Renderer.bounds);
            if (rect == _rect) return;
            
            UpdateTilesInRect(_rect = rect);
        }
        
        private RectInt GetCeilRect(Bounds bounds)
        {
            var min = _tileShape.TileMap.WorldToCell(bounds.min).ToVector2Int();
            var max = _tileShape.TileMap.WorldToCell(bounds.max).ToVector2Int();
            return new RectInt(min, max - min);
        }
        
        private void UpdateTilesInRect(RectInt rect)
        {
            var index = 0;
            Vector2Int size = rect.size;
            var tiles = new TileBase[size.x * size.y];
            for (int ceilY = rect.yMin; ceilY <= rect.yMax; ceilY++)
            {
                for (int ceilX = rect.xMin; ceilX <= rect.xMax; ceilX++)
                {
                    tiles[index++] = GenerateTile(ceilX, ceilY);
                }
            }

            var bounds = new BoundsInt(rect.xMin, rect.yMin, 0, size.x, size.y, 0);
            _tileShape.TileMap.SetTilesBlock(bounds, tiles);
        }

        private GeneratedTile GenerateTile(int ceilX, int ceilY)
        {
            Vector2 worldPosition = new Vector2Int(ceilX, ceilY) * _cellSize + new Vector2(0.5f, 0.5f);
            var tile = new BitTile();
            
            Span<int> typeCounters = stackalloc int[SolidTypeExtensions.Number];
            for (var y = 0; y < _cellSize.y; y++) 
            for (var x = 0; x < _cellSize.x; x++)
            {
                if (Physics2D.OverlapPoint(worldPosition + new Vector2(x, y), _contactFilter, _colliders) > 0)
                {
                    tile[x, y] = true;
                }
                
                foreach (Collider2D collider in _colliders)
                {
                    typeCounters[(int)_tileShape.SolidTypes[collider.sharedMaterial]]++;
                }
            }
            
            SolidType type = GetFrequentSolidType(typeCounters);
            
            var currentTile = _tileShape.TileMap.GetTile<GeneratedTile>(new Vector3Int(ceilX, ceilY));
            return CreateInstance<GeneratedTile>(); //TODO: set data
        }
        
        private static SolidType GetFrequentSolidType(Span<int> typeCounters)
        {
            var maxIndex = 0;
            int maxValue = typeCounters[0];

            for (var i = 1; i < typeCounters.Length; i++)
            {
                if (typeCounters[i] <= maxValue) continue;
                maxValue = typeCounters[i];
                maxIndex = i;
            }

            return (SolidType)maxIndex;
        }
        
        private Sprite CreateSprite(BitTile bitTile)
        {
            /*for (var y = 0; y < _cellSize.y; y++)
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
            */
            return null;
        }
    }
}
