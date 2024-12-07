using System;
using System.Collections.Generic;
using Tiles.Generator;
using Tiles.SolidTypes;
using Tiles.Storage;
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
        [SerializeField] private TileStorageScriptableObject _tileStorage;
        [SerializeField] private SolidTypesScriptableObject _solidTypes;
        
        private readonly List<Collider2D> _colliders = new();
        
        private RectInt _rect;
        private TileShape _tileShape;
        private Vector2Int _cellSize;
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
            
            _contactFilter.useTriggers = true;
            _contactFilter.SetLayerMask(1 << _tileShape.gameObject.layer);
        }
        
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            base.OnInspectorGUI();
            if (!EditorGUI.EndChangeCheck()) return;
            
            UpdateColor();
            UpdateMaterial();
        }

        private void UpdateColor() => Renderer.color = _tileShape.SolidType.ToColor();
        
        private void UpdateMaterial() => _tileShape.Collider.sharedMaterial = _solidTypes[_tileShape.SolidType];
        
        [Obsolete]
        private void OnTransformChanged()
        {
            if (!_tileShape.transform.hasChanged) return;
            
            OnShapeChanged();
            _tileShape.transform.hasChanged = false;
        }
        
        [Obsolete]
        private void OnShapeChanged()
        {
            if (!_tileShape.TileMap) return;
            
            RectInt rect = GetCeilRect(Renderer.bounds);
            UpdateTilesInRect(rect);

            if (rect != _rect)
            {
                UpdateTilesInRect(_rect);
                _rect = rect;
            }
            
            _tileStorage.SaveAssets();
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
            
            for (int ceilY = rect.yMin; ceilY < rect.yMax; ceilY++)
            for (int ceilX = rect.xMin; ceilX < rect.xMax; ceilX++)
            {
                tiles[index++] = GetTileInPosition(ceilX, ceilY);
            }

            var bounds = new BoundsInt(rect.xMin, rect.yMin, 0, size.x, size.y, 1);
            _tileShape.TileMap.SetTilesBlock(bounds, tiles);
        } 

        private GeneratedTile GenerateTile(int ceilX, int ceilY)
        {
            Vector2 worldPosition = new Vector2Int(ceilX, ceilY) * _cellSize + new Vector2(0.5f, 0.5f);
            var bitTile = new BitTile();
            
            Span<int> typeCounters = stackalloc int[SolidTypeExtensions.Number];
            
            for (uint y = 0; y < _cellSize.y; y++)
            for (uint x = 0; x < _cellSize.x; x++)
            {
                if (Physics2D.OverlapPoint(worldPosition + new Vector2(x, y), _contactFilter, _colliders) > 0)
                {
                    bitTile[x, y] = true;
                }

                foreach (Collider2D collider in _colliders)
                {
                    typeCounters[(int)_solidTypes[collider.sharedMaterial]]++;
                }
                
                _colliders.Clear();
            }

            return bitTile.IsEmpty ? 
                null : _tileStorage.CreateIfDifferent(ref bitTile, GetFrequentSolidType(typeCounters));
        }

        private GeneratedTile GetTileInPosition(int ceilX, int ceilY)
        {
            GeneratedTile newTile = GenerateTile(ceilX, ceilY);
            var currentTile = _tileShape.TileMap.GetTile<GeneratedTile>(new Vector3Int(ceilX, ceilY));
            
            if (ReferenceEquals(currentTile, null)) return newTile;
            _tileStorage.AddToRemove(currentTile);
            return newTile;
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
    }
}
