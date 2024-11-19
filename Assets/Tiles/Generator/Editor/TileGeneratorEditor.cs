using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using Utilities;
using static Utilities.Editor.CustomEditorUtilities;

namespace Tiles.Generator.Editor
{
    [CustomEditor(typeof(TileGenerator))]
    public class TileGeneratorEditor : UnityEditor.Editor
    {
        private static readonly Color CreationRectangleColor = new(0f, 0f, 1f, 0.1f);
        
        private TileGenerator _tileGenerator;
        private Handles.CapFunction _capFunction;

        private List<Vector2> _vertices;
        
        private bool _isRectangleCreation;
        private Vector2Int _creationStartPosition;
        
        private void OnEnable()
        {
            _tileGenerator = (TileGenerator)target;
            _capFunction = Handles.SphereHandleCap;
            _isRectangleCreation = false;
            _vertices = new List<Vector2>();
        }

        private void OnDisable() => CursorUtilities.ShowInEditor(true);
        
        private void OnSceneGUI()
        {
            Cursor.visible = false;
            HandleInput(Event.current);
            DrawCreationRectangle(Event.current);
            
            //if (Handles.Button(Vector3.zero, Quaternion.identity, 1f, 1f, _capFunction))
            //{
            //    Debug.Log("suhom");
            //}
            //Vector3 point = SceneView.currentDrawingSceneView.camera.WorldToScreenPoint();
            //DeleteSelectedVertex();

            //Vector2 position = GetWorldMousePosition(Event.current);
            //DrawArc(new Vector2(0f, 0f), new Vector2(56f, 16f), position);
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
            CursorUtilities.ShowInEditor(false);
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
            Vector2 mousePosition = GetWorldMousePosition(e);
            mousePosition.x = MathF.Round(mousePosition.x / 16f);
            mousePosition.y = MathF.Round(mousePosition.y / 16f);
            return mousePosition.ToInt() * 16;
        }

        private void CreateRectangle()
        {
            if (!_isRectangleCreation) return;
            _isRectangleCreation = false;
            
            CursorUtilities.ShowInEditor(true);
        }
        
        /*
        private void DrawEllipseArc(
            float centerX, float centerY, float radiusX, float radiusY, Vector3 from, float angle)
        {
            Vector3 position = _tileGenerator.transform.position;

            float ratioX = MathF.Max(1f, radiusX / radiusY);
            float ratioY = MathF.Max(1f, radiusY / radiusX);
            Handles.matrix = Matrix4x4.TRS(position, Quaternion.identity, new Vector3(ratioX, ratioY));

            var center = new Vector3(centerX / ratioX, centerY / ratioY);
            float minRadius = MathF.Min(radiusX, radiusY);
            Handles.DrawWireArc(center, Vector3.forward, from, angle, minRadius);

            Handles.matrix = Matrix4x4.identity;
        }

        private void DrawEllipse(float centerX, float centerY, float radiusX, float radiusY)
        {
            DrawEllipseArc(centerX, centerY, radiusX, radiusY, Vector3.up, 360f);
        }

        private static void DrawArc(float centerX, float centerY, float radius, Vector3 from, float angle)
        {
            Handles.DrawWireArc(new Vector3(centerX, centerY), Vector3.forward, from, angle, radius);
        }

        private static void DrawCircle(float centerX, float centerY, float radius)
        {
            Handles.DrawWireDisc(new Vector3(centerX, centerY), Vector3.forward, radius);
        }
        */
        
        private void DrawArc(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            if (TryCalculateCircle(p1, p2, p3, out Vector2 center, out float radius))
            {
                float angle = GetDegreesBetweenRadii(center, p1, p2);
                Handles.DrawWireArc(center, Vector3.forward, p1 - center, angle, radius);
            }
            else
            {
                Handles.DrawLine(p1, p2);
            }
            
            DrawVertex(p1);
            DrawVertex(p2);
            DrawVertex(p3);
        }
        
        private static bool TryCalculateCircle(Vector2 p1, Vector2 p2, Vector2 p3, out Vector2 center, out float radius)
        {
            float mid1X = (p1.x + p2.x) / 2f;
            float mid1Y = (p1.y + p2.y) / 2f;
            float mid2X = (p2.x + p3.x) / 2f;
            float mid2Y = (p2.y + p3.y) / 2f;

            float dx1 = p1.x - p2.x;
            float dx2 = p2.x - p3.x;
            float dy1 = p2.y - p1.y;
            float dy2 = p3.y - p2.y;
            
            if (MathF.Abs(dy1 * dx2 - dy2 * dx1) < 30f)
            {
                center = default;
                radius = 0;
                return false;
            }
            
            float slope1 = (p1.x - p2.x) / (p2.y - p1.y);
            float slope2 = (p2.x - p3.x) / (p3.y - p2.y);
            
            center.x = (slope1 * mid1X - slope2 * mid2X + mid2Y - mid1Y) / (slope1 - slope2);
            center.y = slope1 * (center.x - mid1X) + mid1Y;
            
            float relativeX = center.x - p1.x;
            float relativeY = center.y - p1.y;
            radius = MathF.Sqrt(relativeX * relativeX + relativeY * relativeY);
            
            return true;
        }
        
        private static float GetDegreesBetweenRadii(Vector2 center, Vector2 p1, Vector2 p2)
        {
            Vector2 v1 = p1 - center;
            Vector2 v2 = p2 - center;
            
            float dotProduct = v1.x * v2.x + v1.y * v2.y;
            
            float length1 = MathF.Sqrt(v1.x * v1.x + v1.y * v1.y);
            float length2 = MathF.Sqrt(v2.x * v2.x + v2.y * v2.y);
            float sign = v1.x * v2.y - v1.y * v2.x >= 0f ? 1f : -1f;
            
            return sign * MathF.Acos(dotProduct / (length1 * length2)) * Mathf.Rad2Deg;
        }

        private void DrawVertex(Vector3 position)
        {
            Handles.Button(position, Quaternion.identity, 1f, 1f, _capFunction);
        }
    }
}
