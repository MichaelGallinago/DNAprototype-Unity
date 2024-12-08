using Tiles.Models;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Factories
{
    public class TileColliderFactory : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemapFront;
        [SerializeField] private Tilemap _tilemapBack;
        
        public Tiles.Collision.TileCollider Create(Layers layer, Quadrant quadrant, Vector2Int position)
        {
            Tilemap tileMap = layer switch
            {
                Layers.Front => _tilemapFront,
                Layers.Back => _tilemapBack,
                _ => null
            };
            
            return new Tiles.Collision.TileCollider(tileMap, quadrant, position);
        }
    }
}