using Character.Input;
using Tiles.Collision.TileSensorEntity;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Utilities;

namespace Character
{
    [BurstCompile]
    [UpdateAfter(typeof(PhysicsSystem))]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial struct CharacterCollisionSystem : ISystem
    {
        private ComponentLookup<TileSensor> _tileSensorLookup;

        public void OnCreate(ref SystemState state)
        {
            _tileSensorLookup = state.GetComponentLookup<TileSensor>(true);
            state.RequireForUpdate<Character.FloorSensors>();
        }
        
        public void OnUpdate(ref SystemState state)
        {
            _tileSensorLookup.Update(ref state);
            new CharacterCollisionJob
            {
                SensorLookup = _tileSensorLookup 
            }.Schedule();
        }
        
        public void OnDestroy(ref SystemState state) {}
    }
    
    [BurstCompile]
    public partial struct CharacterCollisionJob : IJobEntity 
    {
        private const int MinTolerance = 4;
        private const int MaxTolerance = 14;
        
        [ReadOnly] public ComponentLookup<TileSensor> SensorLookup;
        
        private void Execute(Character character)
        {
            TileSensor sensor = FindClosestSensor(character.FloorSensor);
            if (character.IsGrounded)
            {
                Debug.Log("Ground");
                if (sensor.Distance < -MaxTolerance) return;
                if (TryGoAirborne(character, sensor.Distance))
                {
                    Debug.Log("GoAirborne");
                    character.IsGrounded = false;
                    return;
                }
                
                ApplySensorData(character, sensor);
                return;
            }

            if (sensor.IsInside)
            {
                Debug.Log("Land");
                Land(character, sensor);
            }
            Debug.Log("Air");
        }

        private static bool TryGoAirborne(in Character character, int distance)
        {
            var toleranceCheckSpeed = (int)math.length(character.Velocity);
            int tolerance = math.min(MinTolerance + toleranceCheckSpeed, MaxTolerance);
            return distance > tolerance;
        }
        
        private static void Land(in Character character, in TileSensor sensor)
        {
            //TODO: check if deltaTime transition needed
            character.IsGrounded = true;
            character.GroundSpeed = MathUtilities.ProjectOnPlane(character.Velocity, sensor.Angle);
            character.Velocity = default;
            ApplySensorData(character, sensor);
        }

        private TileSensor FindClosestSensor(in Character.FloorSensors floorSensor)
        {
            TileSensor firstSensor = SensorLookup[floorSensor.First];
            TileSensor secondSensor = SensorLookup[floorSensor.Second];
            return firstSensor.Distance <= secondSensor.Distance ? firstSensor : secondSensor;
        }

        private static void ApplySensorData(in Character character, in TileSensor sensor)
        {
            character.Angle = sensor.Angle;
            character.Position += sensor.OffsetVector;
        }
    }
}
