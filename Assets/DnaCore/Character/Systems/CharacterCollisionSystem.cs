using DnaCore.Character.Components;
using DnaCore.PhysicsEcs2D.Components;
using DnaCore.PhysicsEcs2D.Tiles.Collision;
using DnaCore.PhysicsEcs2D.Tiles.Collision.TileSensorEntity;
using DnaCore.Utilities;
using DnaCore.Utilities.Mathematics;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using static DnaCore.Character.Systems.CollisionJobUtilities;

namespace DnaCore.Character.Systems
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
        
        public readonly void OnDestroy(ref SystemState state) {}
    }

    [BurstCompile]
    [WithAll(typeof(AirTag))]
    [WithPresent(typeof(LandEvent))]
    public partial struct AirCollisionJob : IJobEntity
    {
        [ReadOnly] public ComponentLookup<TileSensor> SensorLookup;
        
        private void Execute(
            ref BehaviourTree behaviour, ref LandEvent landEvent,
            in FloorSensors floorSensors,
            EnabledRefRW<LandEvent> isLandEventEnabled)
        {
            TileSensor sensor = FindClosest(ref SensorLookup, in floorSensors);

            if (!sensor.IsInside) return;
            behaviour.Current = Behaviours.Ground;
            landEvent.Sensor = sensor;
            isLandEventEnabled.ValueRW = true;
        }
    }

    [BurstCompile]
    [WithAll(typeof(GroundTag))]
    public partial struct GroundCollisionJob : IJobEntity 
    {
        private const int MinTolerance = 4;
        private const int MaxTolerance = 14;
        
        [ReadOnly] public ComponentLookup<TileSensor> SensorLookup;
        
        private void Execute(
            ref Rotation rotation, ref LocalTransform transform, ref BehaviourTree behaviour,
            in Velocity velocity, in FloorSensors floorSensors)
        {
            TileSensor sensor = FindClosest(ref SensorLookup, in floorSensors);
            
            if (sensor.Distance < -MaxTolerance) return;
            
            if (TryGoAirborne(velocity.Vector, sensor.Distance))
            {
                behaviour.Current = Behaviours.Air;
                return;
            }
            
            ApplySensorData(ref rotation, ref transform, in sensor);
        }
        
        private static bool TryGoAirborne(float2 velocityVector, int distance)
        {
            var toleranceCheckSpeed = (int)math.length(velocityVector);
            int tolerance = math.min(MinTolerance + toleranceCheckSpeed, MaxTolerance);
            return distance > tolerance;
        }
    }

    [BurstCompile]
    public partial struct LandJob : IJobEntity
    {
        // LandEvent with ref because of EnabledRefRW
        private static void Execute(
            ref Velocity velocity, 
            ref Rotation rotation, 
            ref LocalTransform transform, 
            ref GroundSpeed groundSpeed,
            ref LandEvent landEvent,
            EnabledRefRW<LandEvent> isLandEventEnabled)
        {
            //TODO: check if DeltaTime transition needed
            float sector = MathUtilities.GetSector(landEvent.Sensor.Radians, rotation.Radians);
            float angle = sector <= Circle.OneTwelfth ? rotation.Radians : landEvent.Sensor.Radians;

            groundSpeed.Value = MathUtilities.ProjectOnPlane(velocity.Vector, angle);
            velocity.Vector = default;
            ApplySensorData(ref rotation, ref transform, in landEvent.Sensor);
            isLandEventEnabled.ValueRW = false;
        }
    }
    
    [BurstCompile]
    public static class CollisionJobUtilities
    {
        public static TileSensor FindClosest(ref ComponentLookup<TileSensor> sensorLookup, in FloorSensors floorSensors)
        {
            TileSensor firstSensor = sensorLookup[floorSensors.First];
            TileSensor secondSensor = sensorLookup[floorSensors.Second];
            return firstSensor.Distance <= secondSensor.Distance ? firstSensor : secondSensor;
        }
        
        public static void ApplySensorData(ref Rotation rotation, ref LocalTransform transform, in TileSensor sensor)
        {
            rotation.Radians = sensor.Radians;
            transform.Position.xy = sensor.AddOffset(transform.Position.xy);
        }
    }
}
