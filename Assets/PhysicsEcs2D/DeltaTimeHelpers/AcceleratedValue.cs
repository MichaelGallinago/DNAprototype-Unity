using System;
using Unity.Burst;
using Unity.Mathematics;
using Utilities;

namespace PhysicsEcs2D.DeltaTimeHelpers
{
    [BurstCompile]
    public struct AcceleratedValue : IEquatable<AcceleratedValue>
    {
        private float _value;
        private float _instantValue;
        
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        public bool IsAccelerated => _value != _instantValue;
        
        public AcceleratedValue(float value)
        {
            _value = value;
            _instantValue = value;
        }
        
        public readonly float GetValueDelta(float deltaTime) =>
            ((deltaTime - 1f) * _instantValue + (deltaTime + 1f) * _value) * 0.5f;
        
        public void AddAcceleration(float acceleration, float deltaTime) => _value += acceleration * deltaTime;
        public void ResetInstantValue() => _instantValue = _value;
        
        public void SetClamp(float min, float max)
        {
            if (_value < min)
            {
                _value = _instantValue = min;
            }
            else if (_value > max)
            {
                _value = _instantValue = max;
            }
        }
        
        public void SetMax(float value)
        {
            if (_value >= value) return;
            _value = _instantValue = value;
        }
        
        public void SetMin(float value)
        {
            if (_value <= value) return;
            _value = _instantValue = value;
        }
        
        public void ApplyFriction(float friction, float deltaTime)
        {
            float sign = math.sign(_value);
            AddAcceleration(-sign * friction, deltaTime);
		    
            switch (sign)
            {
                case  1f: SetMax(0f); break;
                case -1f: SetMin(0f); break;
            }
        }
        
        public void Limit(float value, Direction direction)
        {
            if (direction == Direction.Positive)
            {
                SetMin(value);
                return;
            }
            
            SetMax(value);
        }
        
        public void Modify(float modificator)
        {
            _instantValue += modificator;
            _value += modificator;
        }
        
        public bool Equals(AcceleratedValue other) => 
            _value.Equals(other._value) && _instantValue.Equals(other._instantValue);
        
        public override string ToString() => $"{_value} : {_instantValue}";
        public override bool Equals(object obj) => obj is AcceleratedValue other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(_value, _instantValue);
        public static bool operator ==(AcceleratedValue left, AcceleratedValue right) => left.Equals(right);
        public static bool operator !=(AcceleratedValue left, AcceleratedValue right) => !(left == right);
        public static implicit operator float(AcceleratedValue value) => value._value;
        public static implicit operator AcceleratedValue(float value) => new(value);
    }
}
