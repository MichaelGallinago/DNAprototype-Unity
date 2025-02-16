using System;
using DnaCore.Utilities;
using DnaCore.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

namespace DnaCore.PhysicsEcs2D.Tiles.Generators.Editor
{
    [CustomEditor(typeof(ShapeGrid))]
    public class ShapeGridEditor : UnityEditor.Editor
    {
        private static readonly Color CreationRectangleColor = new(0f, 0f, 1f, 0.1f);
        
        [SerializeField] private GameObject _tileShapePrefab;
        
        private ShapeGrid _shapeGrid;
        
        private bool _isRectangleCreation;
        private Vector2Int _creationStartPosition;
        private bool _isShiftPressed;
        
        private void OnEnable()
        {
            _shapeGrid = (ShapeGrid)target;
            _shapeGrid.SetVisibility(true);
        }

        private void OnDisable() => _shapeGrid.SetVisibility(false);
        private void OnSceneGUI() => HandleEvent(Event.current);
        
        private void HandleEvent(Event e)
        {
            switch (e.type)
            {
                case EventType.KeyUp: HandleKeyUp(e); break;
                case EventType.KeyDown: HandleKeyDown(e); break;
                case EventType.Repaint: DrawCreationRectangle(e); break;
                case EventType.MouseMove: SceneView.currentDrawingSceneView.Repaint(); break;
                default: return;
            }
        }

        private void HandleKeyUp(Event e)
        {
            switch (e.keyCode)
            {
                case KeyCode.LeftShift: CreateRectangle(e); break;
                default: return;
            }
            
            e.Use();
        }

        private void HandleKeyDown(Event e)
        {
            switch (e.keyCode)
            {
                case KeyCode.LeftShift: StartRectangleCreation(e); break;
                default: return;
            }
            
            e.Use();
        }

        private void StartRectangleCreation(Event e)
        {
            if (_isRectangleCreation) return;
            
            _creationStartPosition = GetGridPosition(e);
            _isRectangleCreation = true;
        }
        
        private void DrawCreationRectangle(Event e)
        {
            if (!_isRectangleCreation) return;
            
            Vector2Int gridPosition = GetGridPosition(e);
            
            Vector2 position = _creationStartPosition;
            var rectangle = new Rect(position, gridPosition - _creationStartPosition);
            Handles.DrawSolidRectangleWithOutline(rectangle, CreationRectangleColor, Color.clear);

            var endPosition = new Vector3(gridPosition.x, gridPosition.y);
            Handles.DrawSolidDisc(position, Vector3.forward, 1f);
            Handles.DrawSolidDisc(endPosition, Vector3.forward, 1f);
            Handles.DrawSolidDisc(new Vector3(position.x, endPosition.y), Vector3.forward, 1f);
            Handles.DrawSolidDisc(new Vector3(endPosition.x, position.y), Vector3.forward, 1f);
        }

        private static Vector2Int GetGridPosition(Event e)
        {
            Vector2 mousePosition = CustomEditorUtilities.GetWorldMousePosition(e);
            mousePosition.x = MathF.Round(mousePosition.x / TileConstants.HalfSize);
            mousePosition.y = MathF.Round(mousePosition.y / TileConstants.HalfSize);
            return mousePosition.ToInt() * TileConstants.HalfSize;
        }

        private void CreateRectangle(Event e)
        {
            if (!_isRectangleCreation) return;
            _isRectangleCreation = false;
            
            if (PrefabUtility.InstantiatePrefab(_tileShapePrefab) is not GameObject tileShape) return;
            
            Vector2Int position = _creationStartPosition;
            Vector2Int endPosition = GetGridPosition(e);
            Vector2Int halfSize = (endPosition - position).Abs() / 2;
            
            if (halfSize.x <= 0 || halfSize.y <= 0) return;
            
            var offset = new Vector2Int(halfSize.x % TileConstants.HalfSize, halfSize.y % TileConstants.HalfSize);

            var controller = tileShape.GetComponent<SpriteShapeController>();
            controller.spline.SetPosition(0, new Vector3(offset.x - halfSize.x, offset.y - halfSize.y));
            controller.spline.SetPosition(1, new Vector3(offset.x - halfSize.x, offset.y + halfSize.y));
            controller.spline.SetPosition(2, new Vector3(offset.x + halfSize.x, offset.y + halfSize.y));
            controller.spline.SetPosition(3, new Vector3(offset.x + halfSize.x, offset.y - halfSize.y));
            
            tileShape.transform.position = ((position + endPosition) / 2 - offset).ToVector3();
            tileShape.transform.parent = _shapeGrid.transform;
        }
    }
}
