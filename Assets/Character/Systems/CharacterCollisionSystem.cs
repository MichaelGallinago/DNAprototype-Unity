using Character.Components;
using PhysicsEcs2D.Tiles.Collision;
using PhysicsEcs2D.Tiles.Collision.TileSensorEntity;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Utilities;

namespace Character.Systems
{
    [BurstCompile]
    [UpdateAfter(typeof(TileSenseSystem))]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial struct CharacterCollisionSystem : ISystem
    {
        private ComponentLookup<TileSensor> _tileSensorLookup;

        public void OnCreate(ref SystemState state)
        {
            _tileSensorLookup = state.GetComponentLookup<TileSensor>(true);
            state.RequireForUpdate<CharacterAspect.FloorSensors>();
        }
        
        public void OnUpdate(ref SystemState state)
        {
            _tileSensorLookup.Update(ref state);
            state.Dependency = new CharacterCollisionJob
            {
                SensorLookup = _tileSensorLookup,
            }.Schedule(state.Dependency);
        }
        
        public void OnDestroy(ref SystemState state) {}
    }
    
    [BurstCompile]
    public partial struct CharacterCollisionJob : IJobEntity 
    {
        private const int MinTolerance = 4;
        private const int MaxTolerance = 14;
        
        [ReadOnly] public ComponentLookup<TileSensor> SensorLookup;
        
        private void Execute(CharacterAspect characterAspect)
        {
            TileSensor sensor = FindClosestSensor(characterAspect.FloorSensor);
            switch (characterAspect.Behaviour.Current)
            {
                case Behaviours.Ground: CollideOnGround(characterAspect, sensor); break;
                case Behaviours.Air: CollideInAir(characterAspect, sensor); break;
            }
        }

        private static void CollideOnGround(in CharacterAspect characterAspect, in TileSensor sensor)
        {
            if (sensor.Distance < -MaxTolerance) return;
            
            if (TryGoAirborne(characterAspect, sensor.Distance))
            {
                characterAspect.Behaviour.Current = Behaviours.Air;
                return;
            }
            
            ApplySensorData(characterAspect, sensor);
        }

        private static bool TryGoAirborne(in CharacterAspect characterAspect, int distance)
        {
            var toleranceCheckSpeed = (int)math.length(characterAspect.Velocity);
            int tolerance = math.min(MinTolerance + toleranceCheckSpeed, MaxTolerance);
            return distance > tolerance;
        }

        private static void CollideInAir(in CharacterAspect characterAspect, in TileSensor sensor)
        {
            if (sensor.IsInside)
            {
                Land(characterAspect, sensor);
            }
        }
        
        private static void Land(in CharacterAspect characterAspect, in TileSensor sensor)
        {
            //TODO: check if deltaTime transition needed
            characterAspect.Behaviour.Current = Behaviours.Ground;
            characterAspect.GroundSpeed.Value = MathUtilities.ProjectOnPlane(characterAspect.Velocity, sensor.Angle);
            characterAspect.Velocity = default;
            ApplySensorData(characterAspect, sensor);
        }

        private TileSensor FindClosestSensor(in CharacterAspect.FloorSensors floorSensor)
        {
            TileSensor firstSensor = SensorLookup[floorSensor.First];
            TileSensor secondSensor = SensorLookup[floorSensor.Second];
            return firstSensor.Distance <= secondSensor.Distance ? firstSensor : secondSensor;
        }

        private static void ApplySensorData(in CharacterAspect characterAspect, in TileSensor sensor)
        {
            characterAspect.GroundSpeed.Angle = sensor.Angle;
            characterAspect.Position = sensor.AddOffset(characterAspect.Position);
        }
    }
}
