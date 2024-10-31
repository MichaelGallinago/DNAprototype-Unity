using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;

namespace IK2D.Limbs
{
    public class AnthroLimb : AbstractLimb
    {
        [FormerlySerializedAs("_startBone")]
        [Header("Transforms")]
        [SerializeField] private Transform _rootBone;
        [SerializeField] private Transform _endBone;
        [SerializeField] private Transform _end;
        [Header("Data")]
        [SerializeField] private Vector2 _targetPosition;
        [field: SerializeField, Range(-180f, 180f)]
        public float RotationLimit { get; private set; }
        
        private float _endRotation;
        
        private float _length1;
        private float _length2;
        private float _lengthSquared1;
        private float _lengthSquared2;
        private float _maxLength;
        private float _minLength;
        
        public override Vector2 TargetPosition
        {
            get => _targetPosition;
            set
            {
                if (_targetPosition == value) return;
                _targetPosition = value;
                UpdateRotation();
            }
        }
        
        public Vector3 RootPosition => _rootBone.position;
        public Vector3 JointPosition => _endBone.position;
        public Vector3 EndPosition => _end.position;
        public float JointRotation => _endBone.localEulerAngles.z;

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Setup()
        {
            Vector2 endBonePosition = _endBone.position;
            _length1 = VectorUtilities.Distance(_rootBone.position, endBonePosition);
            _length2 = VectorUtilities.Distance(endBonePosition, _end.position);
            _maxLength = _length1 + _length2;
            _lengthSquared1 = _length1 * _length1;
            _lengthSquared2 = _length2 * _length2;
            
            float angle = (180f - MathF.Abs(RotationLimit)) * Mathf.Deg2Rad;
            float distance = _lengthSquared1 + _lengthSquared2 - 2 * _length1 * _length2 * MathF.Cos(angle);
            _minLength = MathF.Sqrt(Math.Max(0f, distance));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float GetAngle(float cos)
        {
            float angle = MathF.Acos(cos);
            return float.IsNaN(angle) ? 0f : angle;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateRotation()
        {
            Vector2 rootPosition = RootPosition;
            float squaredDistance = Mathf.Clamp(VectorUtilities.SquaredDistance(rootPosition, TargetPosition), _minLength * _minLength, _maxLength * _maxLength);
            float distance = MathF.Floor(MathF.Sqrt(squaredDistance));
            
            float angle1 = GetAngle(Math.Clamp((_lengthSquared1 + squaredDistance - _lengthSquared2) / (2 * _length1 * distance), -1f, 1f));
            float angle2 = GetAngle(Math.Clamp((_lengthSquared1 + _lengthSquared2 - squaredDistance) / (2 * _length1 * _length2), -1f, 1f));

            if (RotationLimit > 0)
            {
                angle1 = -angle1;
                angle2 = -angle2;
            }
            
            angle1 += MathF.Atan2(TargetPosition.y - rootPosition.y, TargetPosition.x - rootPosition.x);
            
            _rootBone.localEulerAngles = new Vector3(0f, 0f, angle1 * Mathf.Rad2Deg + 90f);
            _endBone.localEulerAngles = new Vector3(0f, 0f, angle2 * Mathf.Rad2Deg - 180f);
        }
    }
}
