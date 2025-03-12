using System;
using UnityEngine;

namespace DnaCore.IK2D.Heads
{
    public class AnthroHead : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private Transform _eyeTransform; 
        [field: SerializeField, Range(0f, 180f)] 
        public float RotationPositiveLimit { get; private set; }
        [field: SerializeField, Range(-180f, 0f)] 
        public float RotationNegativeLimit { get; private set; }
        [SerializeField] private Vector2 _targetPosition;
        
        public Vector2 EyePosition => _eyeTransform.position;
        public Vector2 Position => _transform.position;
        
        public Vector2 TargetPosition
        {
            get => _targetPosition;
            set
            {
                if (_targetPosition == value) return;
                _targetPosition = value;
                UpdateRotation();
            }
        }
        
        private void UpdateRotation()
        {
            Vector2 lookPosition = _targetPosition - (Vector2)_eyeTransform.position;
            float angle = MathF.Atan2(lookPosition.y, lookPosition.x) * Mathf.Rad2Deg;
            angle = Mathf.Clamp(angle, RotationNegativeLimit, RotationPositiveLimit);
            _transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }
}
