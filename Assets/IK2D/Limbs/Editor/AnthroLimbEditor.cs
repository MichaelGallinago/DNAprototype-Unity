using System.Runtime.CompilerServices;
using Extensions;
using UnityEditor;
using UnityEngine;

namespace IK2D.Limbs.Editor
{
    [CustomEditor(typeof(AnthroLimb))]
    public class AnthroLimbEditor : UnityEditor.Editor
    {
        private Cached<AnthroLimb> _anthroLimb;
        
        private void OnSceneGUI()
        {
            _anthroLimb.Target = target;
            
            DrawGizmos();
            
            AnthroLimb anthroLimb = _anthroLimb;
            EditorGUI.BeginChangeCheck();
            Vector3 newPosition = Handles.PositionHandle(anthroLimb.TargetPosition, Quaternion.identity);
            if (!EditorGUI.EndChangeCheck()) return;

            var undoName = $"{anthroLimb.name} {nameof(anthroLimb.TargetPosition)} changed to {newPosition.ToString()}";
            Undo.RecordObject(anthroLimb, undoName);
            anthroLimb.TargetPosition = newPosition;
            anthroLimb.UpdateLimb();
        }
        
        private void DrawGizmos()
        {
            AnthroLimb anthroLimb = _anthroLimb;
            Vector3 startPosition = anthroLimb.StartPosition;
            Vector3 jointPosition = anthroLimb.JointPosition;
            Vector3 endPosition = anthroLimb.EndPosition;
            
            //Debug.Log($"{startPosition}, {joinPosition}, {endPosition}");
            
            DrawColoredLine(startPosition, endPosition, Color.green);
            DrawColoredLine(startPosition, jointPosition, Color.red);
            DrawColoredLine(jointPosition, endPosition, Color.blue);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DrawColoredLine(Vector3 p1, Vector3 p2, Color color)
        {
            Handles.color = color;
            Handles.DrawLine(p1, p2);
        }
    }
}
