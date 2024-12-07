using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

namespace Tiles.Generator
{
#if UNITY_EDITOR
    [ExecuteAlways]
    [RequireComponent(typeof(Tilemap))]
#endif
    public class TileBaker : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private List<TileShape> _tileShapes = new();
        
        [field: SerializeField, HideInInspector] public Tilemap TileMap { get; private set; }
        
        public IEnumerable<TileShape> TileShapes => _tileShapes;
        
        private void Awake() => UpdateSpriteShapeList();
        private void OnEnable() => UpdateSpriteShapeList();
        private void OnTransformChildrenChanged() => UpdateSpriteShapeList();
        
        private void UpdateSpriteShapeList()
        {
            _tileShapes.Clear();
            
            foreach (Transform child in transform)
            {
                var tileShape = child.GetComponent<TileShape>();
                if (tileShape is null) continue;
                _tileShapes.Add(tileShape);
            }
        }

        private void OnValidate()
        {
            if (!TileMap)
            {
                TileMap = GetComponent<Tilemap>();
            }
        }
    }
#endif
}
