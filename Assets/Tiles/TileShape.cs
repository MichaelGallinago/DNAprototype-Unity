#if UNITY_EDITOR
using Tiles.SizeMapStorage;
using Tiles.SolidTypes;
using Tiles.Storage;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;

namespace Tiles
{
    [RequireComponent(typeof(SpriteShapeController))]
    [RequireComponent(typeof(PolygonCollider2D))]
    public class TileShape : MonoBehaviour
    {
        [field: SerializeField] public SizeMapStorageScriptableObject SizeMaps { get; private set; }
        [field: SerializeField] public SolidTypesScriptableObject SolidTypes { get; private set; }
        [field: SerializeField] public Texture2D TileTexture { get; private set; }
        [field: SerializeField] public Tilemap TileMap { get; private set; }
        [field: SerializeField] public SolidType SolidType { get; private set; }
        [field: SerializeField, HideInInspector] public SpriteShapeController Controller { get; private set; }
        [field: SerializeField, HideInInspector] public PolygonCollider2D Collider { get; private set; }
        
        private void OnValidate()
        {
            if (!Controller)
            {
                Controller = GetComponent<SpriteShapeController>();
            }
            
            if (!Collider)
            {
                Collider = GetComponent<PolygonCollider2D>();
            }
        }
    }
}
#endif
