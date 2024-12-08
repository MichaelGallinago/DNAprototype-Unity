using System;
using UnityEditor;
using UnityEngine;
using Utilities;
using Utilities.Editor;

namespace Tiles.Generators.Editor
{
    [CustomEditor(typeof(ShapeGrid))]
    public class ShapeGridEditor : UnityEditor.Editor
    {
        private static readonly Color CreationRectangleColor = new(0f, 0f, 1f, 0.1f);
        
        private bool _isRectangleCreation;
        private Vector2Int _creationStartPosition;

        private void Awake()
        {
            _isRectangleCreation = false;
        }

        private void OnSceneGUI()
        {
            HandleInput(Event.current);
            DrawCreationRectangle(Event.current);
        }
        
        private void HandleInput(Event e)
        {
            switch (e.type)
            {
                case EventType.KeyUp: HandleKeyUp(e); break;
                case EventType.KeyDown: HandleKeyDown(e); break;
                default: return;
            }
        }

        private void HandleKeyUp(Event e)
        {
            switch (e.keyCode)
            {
                case KeyCode.LeftShift: CreateRectangle(); break;
                default: return;
            }
            
            e.Use();
        }

        private void HandleKeyDown(Event e)
        {
            switch (e.keyCode)
            {
                case KeyCode.Delete: DeleteSelectedVertex(); break;
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
        
        private void DeleteSelectedVertex()
        {
            throw new NotImplementedException();
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
            mousePosition.x = MathF.Round(mousePosition.x / 16f);
            mousePosition.y = MathF.Round(mousePosition.y / 16f);
            return mousePosition.ToInt() * 16;
        }

        private void CreateRectangle()
        {
            if (!_isRectangleCreation) return;
            _isRectangleCreation = false;
        }
    }
}
