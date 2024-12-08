#if UNITY_EDITOR
using Tiles.SolidTypes;
using UnityEngine;
using UnityEngine.U2D;

namespace Tiles
{
    [RequireComponent(typeof(SpriteShapeController))]
    [RequireComponent(typeof(PolygonCollider2D))]
    public class TileShape : MonoBehaviour
    {
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
