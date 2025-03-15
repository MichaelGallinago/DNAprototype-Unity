using DnaCore.Character.Components;
using DnaCore.Character.Input;
using DnaCore.PhysicsEcs2D.Components;
using DnaCore.PhysicsEcs2D.Systems;
using DnaCore.Utilities;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DnaCore.Character.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
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
            in AirLock airLock, in PlayerInput input, in PhysicsData physicsData)
        {
            Rotate(ref rotation);
            velocity.Vector.Clamp(-physicsData.VelocityCap, physicsData.VelocityCap);
            MoveHorizontally(ref velocity, ref rotation, airLock, input, physicsData);
            ApplyDrag(ref velocity);
        }
        
        private static void Rotate(ref Rotation rotation)
        {
            if (Mathf.Approximately(rotation.Angle, 0f)) return;
		
            float speed = Circle.ByteStep * TimeSystem.Speed;
            rotation.Angle += rotation.Angle >= Circle.Half ? speed : -speed;
		
            if (rotation.Angle is < 0f or >= Circle.Full)
            {
                rotation.Angle = 0f;
            }
        }
        
        private static void MoveHorizontally(
            ref Velocity velocity, ref Rotation rotation,
            in AirLock airLock, in PlayerInput input, in PhysicsData physicsData)
        {
            if (airLock.IsLocked) return;
		
            if (input.Down.Left)
            {
                MoveTo(ref velocity, ref rotation, physicsData, Direction.Negative);
            }
            else if (input.Down.Right)
            {
                MoveTo(ref velocity, ref rotation, physicsData, Direction.Positive);
            }
        }
        
        private static void MoveTo(
            ref Velocity velocity, ref Rotation rotation, 
            in PhysicsData physicsData,
            Direction direction)
        {
            var sign = (int)direction;
            float speed = sign * velocity.Vector.X;
            float acceleration = sign * physicsData.AccelerationAir;

            rotation.Facing = direction;
            
            if (speed < 0f)
            {
                velocity.Vector.X.AddAcceleration(acceleration, TimeSystem.Speed);
                return;
            }

            if (speed >= physicsData.AccelerationTop) return;
            
            velocity.Vector.X.AddAcceleration(acceleration, TimeSystem.Speed);
            velocity.Vector.X.Limit(sign * physicsData.AccelerationTop, direction);
        }

        private static void ApplyDrag(ref Velocity velocity)
        {
            if ((float)velocity.Vector.Y is <= 0f or >= 4f) return;
            
            float acceleration = math.floor(velocity.Vector.X * 8f) / -256f;
            velocity.Vector.X.AddAcceleration(acceleration, TimeSystem.Speed);
        }
    }

    [BurstCompile]
    [WithAll(typeof(GroundTag))]
    public partial struct GroundMoveJob : IJobEntity
    {
        private static void Execute(
            ref Rotation rotation, ref GroundSpeed groundSpeed, ref Velocity velocity,
            in PlayerInput input, in PhysicsData physicsData)
        {
            if (input.Down.Right)
            {
                WalkOnGround(ref rotation, ref groundSpeed, in physicsData, Direction.Positive);
            }
            else if (input.Down.Left)
            {
                WalkOnGround(ref rotation, ref groundSpeed, in physicsData, Direction.Negative);
            }
            else
            {
                groundSpeed.Value.ApplyFriction(physicsData.Friction, TimeSystem.Speed);
            }
            
            velocity.Vector.SetDirectionalValue(groundSpeed.Value, rotation.Angle);
        }
        
        private static void WalkOnGround(
            ref Rotation rotation, ref GroundSpeed groundSpeed, 
            in PhysicsData physicsData,
            Direction direction)
        {
            var sign = (float)direction;
        
            if (groundSpeed.Value * sign < 0f)
            {
                groundSpeed.Value.AddAcceleration(sign * physicsData.Deceleration, TimeSystem.Speed);
                if (direction == Direction.Positive == groundSpeed.Value >= 0f)
                {
                    groundSpeed.Value = 0.5f * sign;
                }
                return;
            }
            
            groundSpeed.Value.AddAcceleration(sign * physicsData.Acceleration, TimeSystem.Speed);
        
            switch (direction)
            {
                case Direction.Positive: groundSpeed.Value.SetMin( physicsData.AccelerationTop); break;
                case Direction.Negative: groundSpeed.Value.SetMax(-physicsData.AccelerationTop); break;
            }
            
            rotation.Facing = direction;
        }
    }
}
