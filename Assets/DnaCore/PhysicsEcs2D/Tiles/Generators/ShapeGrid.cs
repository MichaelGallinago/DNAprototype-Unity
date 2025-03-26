#if UNITY_EDITOR
using DnaCore.PhysicsEcs2D.Tiles.Generators.TilemapGenerator;
using UnityEditor;
using UnityEngine;

namespace DnaCore.PhysicsEcs2D.Tiles.Generators
{
    public class ShapeGrid : MonoBehaviour
    {
        public void SetVisibility(bool isVisible)
        {
            var baker = GetComponentInParent<TilemapAuthoring>();
            if (!baker.TilemapRenderer) return;
            baker.TilemapRenderer.enabled = !isVisible;

            if (isVisible)
            {
                SceneVisibilityManager.instance.Show(gameObject, true);
            }
            else
            {
                SceneVisibilityManager.instance.Hide(gameObject, true);
            }
        }
    }
}
#endif
