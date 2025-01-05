using Character.Components;
using PhysicsEcs2D.Tiles.Collision;
using PhysicsEcs2D.Tiles.Collision.TileSensorEntity;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Utilities;
using static Character.Systems.CollisionJobUtilities;

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
            state.RequireForUpdate<FloorSensors>();
        }
        
        public void OnUpdate(ref SystemState state)
        {
            _tileSensorLookup.Update(ref state);
            state.Dependency = new AirCollisionJob { SensorLookup = _tileSensorLookup }.Schedule(state.Dependency);
            state.Dependency = new GroundCollisionJob { SensorLookup = _tileSensorLookup }.Schedule(state.Dependency);
            state.Dependency = new LandJob().Schedule(state.Dependency);
        }
        
        public void OnDestroy(ref SystemState state) {}
    }

    [BurstCompile]
    public partial struct AirCollisionJob : IJobEntity
    {
        [ReadOnly] public ComponentLookup<TileSensor> SensorLookup;
        
        private void Execute(CharacterAspect character, AirBehaviourAspect air, ref LandEvent landEvent)
        {
            TileSensor sensor = FindClosest(character.FloorSensor, ref SensorLookup);
            
            if (sensor.IsInside)
            {
                character.Behaviour.Current = Behaviours.Ground;
                landEvent.Sensor = sensor;
            }
        }
    }

    [BurstCompile]
    public partial struct GroundCollisionJob : IJobEntity 
    {
        private const int MinTolerance = 4;
        private const int MaxTolerance = 14;
        
        [ReadOnly] public ComponentLookup<TileSensor> SensorLookup;
        
        private void Execute(CharacterAspect character, GroundBehaviourAspect ground)
        {
            TileSensor sensor = FindClosest(character.FloorSensor, ref SensorLookup);
            
            if (sensor.Distance < -MaxTolerance) return;
            
            if (TryGoAirborne(character, sensor.Distance))
            {
                character.Behaviour.Current = Behaviours.Air;
                return;
            }
            
            ApplySensorData(character, sensor);
        }
        
        private static bool TryGoAirborne(in CharacterAspect characterAspect, int distance)
        {
            var toleranceCheckSpeed = (int)math.length(characterAspect.Velocity);
            int tolerance = math.min(MinTolerance + toleranceCheckSpeed, MaxTolerance);
            return distance > tolerance;
        }
    }

    [BurstCompile]
    public partial struct LandJob : IJobEntity
    {
        private static void Execute(CharacterAspect character, GroundBehaviourAspect ground, [WithChangeFilter] in LandEvent landEvent)
        {
            //TODO: check if deltaTime transition needed
            //Debug.Log("Hello");
            ground.Speed = MathUtilities.ProjectOnPlane(character.Velocity, landEvent.Sensor.Angle);
            character.Velocity = default;
            ApplySensorData(character, landEvent.Sensor);
        }
    }
    
    [BurstCompile]
    public static class CollisionJobUtilities
    {
        public static TileSensor FindClosest(in FloorSensors floorSensors, ref ComponentLookup<TileSensor> sensorLookup)
        {
            TileSensor firstSensor = sensorLookup[floorSensors.First];
            TileSensor secondSensor = sensorLookup[floorSensors.Second];
            return firstSensor.Distance <= secondSensor.Distance ? firstSensor : secondSensor;
        }
        
        public static void ApplySensorData(in CharacterAspect character, in TileSensor sensor)
        {
            character.Rotation.Angle = sensor.Angle;
            character.Position = sensor.AddOffset(character.Position);
        }
    }
}
