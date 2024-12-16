using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

namespace Character
{
    [BurstCompile]
    public struct AcceleratedVector2
    {
        public AcceleratedValue X;
        public AcceleratedValue Y;
        
        public AcceleratedVector2(Vector2 vector) 
        {
            X = vector.x;
            Y = vector.y;
        }
        
        public AcceleratedVector2(float2 vector) 
        {
            X = vector.x;
            Y = vector.y;
        }
        
        public AcceleratedVector2(float x, float y)
        {
            X = x;
            Y = y;
        }
        
        public readonly float2 GetValueDelta(float deltaTime) => 
            new(X.GetValueDelta(deltaTime), Y.GetValueDelta(deltaTime));
        
        public void ResetInstanceValue()
        {
            X.ResetInstantValue();
            Y.ResetInstantValue();
        }
        
        public void AddAcceleration(float2 acceleration, float deltaTime)
        {
            X.AddAcceleration(acceleration.x, deltaTime);
            Y.AddAcceleration(acceleration.y, deltaTime);
        }
        
        public void Modify(float2 modificator)
        {
            X.Modify(modificator.x);
            Y.Modify(modificator.y);
        }
        
        public void Clamp(float2 min, float2 max)
        {
            X.SetClamp(min.x, max.x);
            Y.SetClamp(min.y, max.y);
        }
        
        public void Min(float2 value)
        {
            X.SetMin(value.x);
            Y.SetMin(value.y);
        }
        
        public void Max(float2 value)
        {
            X.SetMax(value.x);
            Y.SetMax(value.y);
        }
        
        public void SetDirectionalValue(AcceleratedValue value, float angle)
        {
            float floatValue = value;
            float radians = angle * math.TORADIANS;
            X = floatValue * math.cos(radians);
            Y = floatValue * -math.sin(radians);
            //TODO: check IsAccelerated
            //if (value.IsAccelerated) return; 
            ResetInstanceValue();
        }
        
        public override string ToString() => $"{{{X.ToString()}; {Y.ToString()}}}";
        public static implicit operator float2(AcceleratedVector2 vector) => new(vector.X, vector.Y);
        public static implicit operator AcceleratedVector2(float2 vector) => new(vector);
    }
}
