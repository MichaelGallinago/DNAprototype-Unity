using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tiles.Generators
{
    [ExecuteAlways]
    public class ShapeGrid : MonoBehaviour
    {
        private TilemapRenderer _tileMapRenderer;

        private void Awake() => _tileMapRenderer = GetComponentInParent<TilemapRenderer>();
        private void OnEnable() => UpdateVisibility(false);
        private void OnDisable() => UpdateVisibility(true);
        
        private void UpdateVisibility(bool visible)
        {
            _tileMapRenderer.enabled = !visible;

            if (visible)
            {
                SceneVisibilityManager.instance.Show(gameObject, true);
            }
            else
            {
                SceneVisibilityManager.instance.Hide(gameObject, true);
            }
        }
        
        private void Update()
        {
            bool isAnyChildSelected = IsAnyChildSelected();
            UpdateVisibility(isAnyChildSelected);
        }
        
        private bool IsAnyChildSelected()
        {
            GameObject selectedObject = Selection.activeGameObject;
            if (!selectedObject) return false;
            
            return selectedObject == gameObject || selectedObject.transform.IsChildOf(transform);
        }
    }
}
