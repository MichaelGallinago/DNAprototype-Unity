using Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Factories
{
    public class TileColliderFactory : MonoBehaviour
    {
        [SerializeField] private Tilemap _tileMapFront;
        [SerializeField] private Tilemap _tileMapBack;
        
        public Tiles.Collision.TileCollider Create(Layers layer, Quadrant quadrant, Vector2Int position)
        {
            Tilemap tileMap = layer switch
            {
                Layers.Front => _tileMapFront,
                Layers.Back => _tileMapBack,
                _ => null
            };
            
            return new Tiles.Collision.TileCollider(tileMap, quadrant, position);
        }
    }
}