using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace Utilities
{
    public static class CustomEditorUtilities
    {
        public static Camera CurrentCamera
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => SceneView.currentDrawingSceneView.camera;
        }
        
        public static Vector2 GetWorldMousePosition(Event currentEvent)
        {
            Vector2 point = currentEvent.mousePosition * EditorGUIUtility.pixelsPerPoint;
            Camera camera = CurrentCamera;
            point.y = camera.pixelHeight - point.y;
            return camera.ScreenToWorldPoint(point);
        }
    }
}