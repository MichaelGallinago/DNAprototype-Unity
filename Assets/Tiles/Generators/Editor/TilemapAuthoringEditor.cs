using System;
using System.Collections.Generic;
using Tiles.Models;
using Tiles.SolidTypes;
using Tiles.Storage;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utilities;

namespace Tiles.Generators.Editor
{
    [CustomEditor(typeof(TilemapAuthoring))]
    public class TilemapAuthoringEditor : UnityEditor.Editor
    {
        private static readonly List<Collider2D> Colliders = new();
        
        [SerializeField] private TileStorageScriptableObject _tileStorage;
        [SerializeField] private SolidTypesScriptableObject _solidTypes;
        
        private TilemapAuthoring _baker;
        
        private ContactFilter2D _contactFilter;

        private void Awake()
        {
            _baker = (TilemapAuthoring)target;
            
            _contactFilter.useTriggers = true;
            _contactFilter.SetLayerMask(LayerMask.GetMask("CollisionGeneration"));
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            BakeButton();
            ClearButton();
        }
        
        private void ClearButton()
        {
            if (!GUILayout.Button("Clear")) return;
            
            _baker.Tilemap.CompressBounds();
            _tileStorage.DeleteTilesFromTilemap(_baker.Tilemap.GetTilesBlock(_baker.Tilemap.cellBounds));
            _baker.Tilemap.ClearAllTiles();
            
            EditorUtility.SetDirty(_baker.Tilemap);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void BakeButton()
        {
            if (!GUILayout.Button("Bake")) return;

            RemoveTiles();
            
            foreach (TileShape tileShape in _baker.TileShapes)
            {
                SetTilesInRect(GetCeilRect(tileShape.Controller.spriteShapeRenderer.bounds));
            }
            
            EditorUtility.SetDirty(_baker.Tilemap);
            _tileStorage.SaveAssets();
                        
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void RemoveTiles()
        {
            _baker.Tilemap.CompressBounds();
            
            BoundsInt bounds = _baker.Tilemap.cellBounds;
            TileBase[] allTiles = _baker.Tilemap.GetTilesBlock(bounds);
            
            int size = bounds.size.x * bounds.size.y;
            for (var i = 0; i < size; i++)
            {
                if (allTiles[i] is not GeneratedTile tile) continue;
                _tileStorage.AddToRemove(tile);
            }
            
            _baker.Tilemap.ClearAllTiles();
        }
        
        private void SetTilesInRect(RectInt rect)
        {
            var index = 0;
            Vector2Int size = rect.size;
            var tiles = new TileBase[size.x * size.y];
            
            for (int ceilY = rect.yMin; ceilY < rect.yMax; ceilY++)
            for (int ceilX = rect.xMin; ceilX < rect.xMax; ceilX++)
            {
                tiles[index++] = GenerateTile(ceilX, ceilY);
            }

            var bounds = new BoundsInt(rect.xMin, rect.yMin, 0, size.x, size.y, 1);
            _baker.Tilemap.SetTilesBlock(bounds, tiles);
        }
        
        private GeneratedTile GenerateTile(int ceilX, int ceilY)
        {
            Vector2 worldPosition = new Vector2Int(ceilX, ceilY) * TileUtilities.CellSize + new Vector2(0.5f, 0.5f);
            var bitTile = new BitTile();
            
            Span<int> typeCounters = stackalloc int[SolidTypeExtensions.Number];
            
            for (uint y = 0; y < TileUtilities.Size; y++)
            for (uint x = 0; x < TileUtilities.Size; x++)
            {
                if (Physics2D.OverlapPoint(worldPosition + new Vector2(x, y), _contactFilter, Colliders) > 0)
                {
                    bitTile[x, y] = true;
                }

                foreach (Collider2D collider in Colliders)
                {
                    typeCounters[(int)_solidTypes[collider.sharedMaterial]]++;
                }
                
                Colliders.Clear();
            }

            return bitTile.IsEmpty ? 
                null : _tileStorage.CreateIfDifferent(ref bitTile, GetFrequentSolidType(typeCounters));
        }
        
        private RectInt GetCeilRect(Bounds bounds)
        {
            var min = _baker.Tilemap.WorldToCell(bounds.min).ToVector2Int();
            var max = _baker.Tilemap.WorldToCell(bounds.max).ToVector2Int();
            return new RectInt(min, max - min + Vector2Int.one);
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
