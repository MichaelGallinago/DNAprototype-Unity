using UnityEditor;
using UnityEngine;

namespace Tiles.Generators
{
    public class ShapeGrid : MonoBehaviour
    {
#if UNITY_EDITOR
        public void SetVisibility(bool isVisible)
        {
            var baker = GetComponentInParent<TileBaker>();
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
#endif
    }
}
