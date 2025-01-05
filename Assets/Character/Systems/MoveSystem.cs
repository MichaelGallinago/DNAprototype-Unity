using Character.Input;
using PhysicsEcs2D;
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
    public partial struct AirMoveJob : IJobEntity
    {
        private static void Execute(CharacterAspect character, AirBehaviourAspect air, in PlayerInput input)
        {
            Rotate(character);
            character.Velocity.Clamp(-Constants.VelocityCap, Constants.VelocityCap);
            MoveHorizontally(character, air, input);
            ApplyDrag(character);
        }
        
        private static void Rotate(in CharacterAspect character)
        {
            if (Mathf.Approximately(character.Rotation.Angle, 0f)) return;
		
            float speed = Circle.ByteStep * Constants.Speed;
            character.Rotation.Angle += character.Rotation.Angle >= Circle.Half ? speed : -speed;
		
            if (character.Rotation.Angle is < 0f or >= Circle.Full)
            {
                character.Rotation.Angle = 0f;
            }
        }
        
        private static void MoveHorizontally(
            in CharacterAspect character, in AirBehaviourAspect air, in PlayerInput input)
        {
            if (air.IsLocked) return;
		
            if (input.Down.Left)
            {
                MoveTo(character, Direction.Negative);
            }
            else if (input.Down.Right)
            {
                MoveTo(character, Direction.Positive);
            }
        }
        
        private static void MoveTo(in CharacterAspect character, Direction direction)
        {
            var sign = (int)direction;
            float speed = sign * character.Velocity.X;
            float acceleration = sign * Constants.AccelerationAir;
		
            switch (speed)
            {
                case < 0f:
                    character.Velocity.X.AddAcceleration(acceleration, Constants.Speed);
                    break;
                case < Constants.AccelerationTop:
                    character.Velocity.X.AddAcceleration(acceleration, Constants.Speed);
                    character.Velocity.X.Limit(sign * Constants.AccelerationTop, direction);
                    break;
            }

            character.Rotation.Facing = direction;
        }

        private static void ApplyDrag(in CharacterAspect character)
        {
            if ((float)character.Velocity.Y is < 0f and > -4f)
            {
                float acceleration = math.floor(character.Velocity.X * 8f) / -256f;
                character.Velocity.X.AddAcceleration(acceleration, Constants.Speed);
            }
        }
    }

    [BurstCompile]
    public partial struct GroundMoveJob : IJobEntity
    {
        private static void Execute(CharacterAspect character, GroundBehaviourAspect ground, in PlayerInput input)
        {
            if (input.Down.Right)
            {
                WalkOnGround(character, ground, Direction.Positive);
            }
            else if (input.Down.Left)
            {
                WalkOnGround(character, ground, Direction.Negative);
            }
            else
            {
                ground.Speed.ApplyFriction(Constants.Friction, Constants.Speed);
            }
            
            character.Velocity.SetDirectionalValue(ground.Speed, character.Rotation.Angle);
        }
        
        private static void WalkOnGround(
            in CharacterAspect character, in GroundBehaviourAspect ground, Direction direction)
        {
            var sign = (float)direction;
        
            if (ground.Speed * sign < 0f)
            {
                ground.Speed.AddAcceleration(sign * Constants.Deceleration, Constants.Speed);
                if (direction == Direction.Positive == ground.Speed >= 0f)
                {
                    ground.Speed = 0.5f * sign;
                }
                return;
            }
            
            ground.Speed.AddAcceleration(Constants.Acceleration * sign, Constants.Speed);
        
            switch (direction)
            {
                case Direction.Positive: ground.Speed.SetMin( Constants.AccelerationTop); break;
                case Direction.Negative: ground.Speed.SetMax(-Constants.AccelerationTop); break;
            }
            
            TurnAround(character, direction);
        }
        
        private static void TurnAround(in CharacterAspect character, Direction direction)
        {
            if (character.Rotation.Facing == direction) return;
            character.Rotation.Facing = direction;
        }
    }
}
