using Tiles.Generator;
using Tiles.SolidTypes;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;

namespace Tiles
{
    [RequireComponent(typeof(SpriteShapeController))]
    [RequireComponent(typeof(PolygonCollider2D))]
    public class TileShape : MonoBehaviour
    {
        [field: SerializeField] public SolidType SolidType { get; private set; }
        [field: SerializeField] public LayerMask LayerMask { get; private set; }
        [field: SerializeField] public Tilemap TileMap { get; private set; }
        [field: SerializeField] public SpriteShapeController Controller { get; private set; }
        [field: SerializeField] public PolygonCollider2D Collider { get; private set; }
    }
}
