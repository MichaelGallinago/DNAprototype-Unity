using UnityEditor;
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
        [field: SerializeField, HideInInspector] public Tilemap TileMap { get; private set; }
        [field: SerializeField, HideInInspector] public TilemapRenderer TileMapRenderer { get; private set; }
        
        public TileShape[] TileShapes => GetComponentsInChildren<TileShape>();
        
        private void OnValidate()
        {
            if (!TileMap)
            {
                TileMap = GetComponent<Tilemap>();
            }
            
            if (!TileMapRenderer)
            {
                TileMapRenderer = GetComponent<TilemapRenderer>();
            }
        }
        
        public void SetVisibility(bool isTilesVisible)
        {
            TileMapRenderer.enabled = isTilesVisible;
            if (isTilesVisible)
            {
                SceneVisibilityManager.instance.Hide(_shapeGrid.gameObject, true);
            }
            else
            {
                SceneVisibilityManager.instance.Show(_shapeGrid.gameObject, true);
            }
        }
    }
#endif
}
