using System;
using System.Runtime.CompilerServices;
using Extensions;
using UnityEngine;

namespace IK2D.Limbs
{
    public class AnthroLimb : AbstractLimb
    {
        [SerializeField] private Transform _startBone;
        [SerializeField] private Transform _endBone;
        [SerializeField, Min(0f)] private float _length1;
        [SerializeField, Min(0f)] private float _length2;
        [SerializeField] private Vector2 _endPosition;
        
        private float _cosLength1;
        private float _cosLength2;
        private float _twoLength1;
        private float _lengthDivider;
        private float _endRotation;
        
        public override Vector2 EndPosition
        {
            get => _endPosition;
            set
            {
                if (_endPosition == value) return;
                _endPosition = value;
                UpdateRotation();
            }
        }

        public override Vector2 StartPosition
        {
            get => _startBone.position;
            set
            {
                _startBone.position = value;
                UpdateRotation();
            }
        }

        public override float EndRotation
        {
            get => _endRotation;
            set
            {
                if (Mathf.Approximately(_endRotation, value)) return;
                _endRotation = value;
                _endBone.rotation = Quaternion.Euler(0f, 0f, value);
            }
        }

        private void Start() => Setup();

#if UNITY_EDITOR
        private void OnValidate()
        {
            Setup();
            UpdateRotation();
        }
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Setup()
        {
            float square = _length2 * _length2;
            _cosLength1 = _length1 * _length1;
            _cosLength2 = _cosLength1 + square;
            _cosLength1 -= square;

            _twoLength1 = 2f * _length1;
            _lengthDivider = 1f / (_twoLength1 * _length2);
        }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateRotation()
        {
            float distanceSquared = VectorUtilities.SquaredDistance(_startBone.position, EndPosition);
            float distance = MathF.Sqrt(distanceSquared);
            
            float angle1 = GetAngle((_cosLength1 + distanceSquared) / (_twoLength1 * distance));
            float angle2 = GetAngle((_cosLength2 - distanceSquared) * _lengthDivider);
            
#pragma warning disable CS0618 // Type or member is obsolete
            _startBone.rotation.SetEulerAngles(new Vector3(0f, 0f, angle1));
            _endBone.rotation.SetEulerAngles(new Vector3(0f, 0f, angle2));
#pragma warning restore CS0618 // Type or member is obsolete
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float GetAngle(float cos)
        {
            float angle = MathF.Acos(cos);
            return float.IsNaN(angle) ? 0f : angle;
        }
    }
}
