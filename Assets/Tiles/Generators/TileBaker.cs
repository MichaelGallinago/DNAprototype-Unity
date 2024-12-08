using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tiles.Generators
{
#if UNITY_EDITOR
    [ExecuteAlways]
    [RequireComponent(typeof(Tilemap))]
    [RequireComponent(typeof(TilemapRenderer))]
#endif
    public class TileBaker : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private Grid _shapeGrid;
        [field: SerializeField, HideInInspector] public Tilemap Tilemap { get; private set; }
        [field: SerializeField, HideInInspector] public TilemapRenderer TilemapRenderer { get; private set; }
        
        public TileShape[] TileShapes => GetComponentsInChildren<TileShape>();
        
        private void OnValidate()
        {
            if (!Tilemap)
            {
                Tilemap = GetComponent<Tilemap>();
            }
            
            if (!TilemapRenderer)
            {
                TilemapRenderer = GetComponent<TilemapRenderer>();
            }
        }
    }
#endif
}
