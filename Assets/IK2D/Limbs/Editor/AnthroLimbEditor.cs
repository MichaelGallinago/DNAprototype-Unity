using System;
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
            
            AnthroLimb anthroLimb = _anthroLimb;
            Vector3 startPosition = anthroLimb.StartPosition;
            
            float angle = _startBone.rotation.z * Mathf.Deg2Rad;
            float cosine = MathF.Cos(angle);
            float sine = MathF.Sin(angle);
            Vector3 joinPosition = startPosition + (Vector3)(new Vector2(cosine, sine) * _length1);
            print($"{startPosition}, {joinPosition}, {_endPosition}");
            Debug.DrawLine(startPosition, joinPosition, Color.green);
            Debug.DrawLine(joinPosition, _endPosition, Color.red);
            Debug.DrawLine(startPosition, _endPosition, Color.blue);
            
            Handles.color = Color.red;
            //Handles.DrawLine();
            Handles.color = Color.green;
            //Handles.DrawLine();
            Handles.color = Color.blue;
            //Handles.DrawLine();
        }
    }
}
