using System;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace IK2D.Heads.Editor
{
    [CustomEditor(typeof(AnthroHead))]
    public class AnthroHeadEditor : UnityEditor.Editor
    {
        private AnthroHead _anthroHead;
        
        private void Awake() => _anthroHead = (AnthroHead)target;
        
        private void OnSceneGUI()
        {
            DrawGizmos();
            
            AnthroHead anthroHead = _anthroHead;
            EditorGUI.BeginChangeCheck();
            Vector3 newPosition = Handles.PositionHandle(anthroHead.TargetPosition, Quaternion.identity);
            if (!EditorGUI.EndChangeCheck()) return;

            var undoName = $"{anthroHead.name} {nameof(anthroHead.TargetPosition)} changed to {newPosition.ToString()}";
            Undo.RecordObject(anthroHead, undoName);
            anthroHead.TargetPosition = newPosition;
        }
        
        private void DrawGizmos()
        {
            AnthroHead anthroHead = _anthroHead;
            
            Vector2 position = anthroHead.Position;
            Vector2 targetPosition = anthroHead.TargetPosition;
            DrawColoredLine(anthroHead.EyePosition, targetPosition, Color.green);
            DrawColoredLine(position, targetPosition, Color.blue);

            float negativeLimit = anthroHead.RotationNegativeLimit;
            float angle = negativeLimit - anthroHead.RotationPositiveLimit;
            float radians = (180f + anthroHead.RotationPositiveLimit) * Mathf.Deg2Rad;
            var from = new Vector3(-MathF.Cos(radians), -MathF.Sin(radians), 0f);
            const float radius = 10f;
            
            Handles.color = new Color(1f, 1f, 1f, 0.025f);
            Handles.DrawSolidArc(position, Vector3.forward, from, angle, radius);
            Handles.color = Color.white;
            Handles.DrawWireArc(position, Vector3.forward, from, angle, radius);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DrawColoredLine(Vector3 p1, Vector3 p2, Color color)
        {
            Handles.color = color;
            Handles.DrawLine(p1, p2);
        }
    }
}
