using Character.Components;
using Character.Input;
using PhysicsEcs2D;
using PhysicsEcs2D.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Utilities;

namespace Character.Systems
{
    [BurstCompile]
    public partial struct MoveSystem : ISystem
    {
        public void OnCreate(ref SystemState state) {}

        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new AirMoveJob().ScheduleParallel(state.Dependency);
            state.Dependency = new GroundMoveJob().ScheduleParallel(state.Dependency);
        }
        
        public void OnDestroy(ref SystemState state) {}
    }

    [BurstCompile]
    [WithAll(typeof(AirTag))]
    public partial struct AirMoveJob : IJobEntity
    {
        private static void Execute(
            ref Rotation rotation, ref Velocity velocity, 
            in AirLock airLock, in PlayerInput input)
        {
            Rotate(ref rotation);
            velocity.Vector.Clamp(-Constants.VelocityCap, Constants.VelocityCap);
            MoveHorizontally(ref velocity, ref rotation, airLock, input);
            ApplyDrag(ref velocity);
        }
        
        private static void Rotate(ref Rotation rotation)
        {
            if (Mathf.Approximately(rotation.Angle, 0f)) return;
		
            float speed = Circle.ByteStep * Constants.Speed;
            rotation.Angle += rotation.Angle >= Circle.Half ? speed : -speed;
		
            if (rotation.Angle is < 0f or >= Circle.Full)
            {
                rotation.Angle = 0f;
            }
        }
        
        private static void MoveHorizontally(
            ref Velocity velocity, ref Rotation rotation, 
            in AirLock airLock, in PlayerInput input)
        {
            if (airLock.IsLocked) return;
		
            if (input.Down.Left)
            {
                MoveTo(ref velocity, ref rotation, Direction.Negative);
            }
            else if (input.Down.Right)
            {
                MoveTo(ref velocity, ref rotation, Direction.Positive);
            }
        }
        
        private static void MoveTo(ref Velocity velocity, ref Rotation rotation, Direction direction)
        {
            var sign = (int)direction;
            float speed = sign * velocity.Vector.X;
            float acceleration = sign * Constants.AccelerationAir;
		
            switch (speed)
            {
                case < 0f:
                    velocity.Vector.X.AddAcceleration(acceleration, Constants.Speed);
                    break;
                case < Constants.AccelerationTop:
                    velocity.Vector.X.AddAcceleration(acceleration, Constants.Speed);
                    velocity.Vector.X.Limit(sign * Constants.AccelerationTop, direction);
                    break;
            }

            rotation.Facing = direction;
        }

        private static void ApplyDrag(ref Velocity velocity)
        {
            if ((float)velocity.Vector.Y is >= 0f or <= -4f) return;
            
            float acceleration = math.floor(velocity.Vector.X * 8f) / -256f;
            velocity.Vector.X.AddAcceleration(acceleration, Constants.Speed);
        }
    }

    [BurstCompile]
    [WithAll(typeof(GroundTag))]
    public partial struct GroundMoveJob : IJobEntity
    {
        private static void Execute(
            ref Rotation rotation, ref GroundSpeed groundSpeed, ref Velocity velocity,
            in PlayerInput input)
        {
            if (input.Down.Right)
            {
                WalkOnGround(ref rotation, ref groundSpeed, Direction.Positive);
            }
            else if (input.Down.Left)
            {
                WalkOnGround(ref rotation, ref groundSpeed, Direction.Negative);
            }
            else
            {
                groundSpeed.Value.ApplyFriction(Constants.Friction, Constants.Speed);
            }
            
            velocity.Vector.SetDirectionalValue(groundSpeed.Value, rotation.Angle);
        }
        
        private static void WalkOnGround(ref Rotation rotation, ref GroundSpeed groundSpeed, Direction direction)
        {
            var sign = (float)direction;
        
            if (groundSpeed.Value * sign < 0f)
            {
                groundSpeed.Value.AddAcceleration(sign * Constants.Deceleration, Constants.Speed);
                if (direction == Direction.Positive == groundSpeed.Value >= 0f)
                {
                    groundSpeed.Value = 0.5f * sign;
                }
                return;
            }
            
            groundSpeed.Value.AddAcceleration(Constants.Acceleration * sign, Constants.Speed);
        
            switch (direction)
            {
                case Direction.Positive: groundSpeed.Value.SetMin( Constants.AccelerationTop); break;
                case Direction.Negative: groundSpeed.Value.SetMax(-Constants.AccelerationTop); break;
            }
            
            rotation.Facing = direction;
        }
    }
}
