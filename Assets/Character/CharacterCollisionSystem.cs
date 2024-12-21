using Character.Input;
using Tiles.Collision.TileSensorEntity;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Utilities;

namespace Character
{
    [BurstCompile]
    [UpdateAfter(typeof(PhysicsSystem))]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial struct CharacterCollisionSystem : ISystem
    {
        public void OnCreate(ref SystemState state) =>
            state.RequireForUpdate<CharacterSensors>();

        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new CharacterCollisionJob 
            {
                Sensors = state.GetComponentLookup<TileSensor>(true),
            }.Schedule(state.Dependency);
        }
        
        public void OnDestroy(ref SystemState state) {}
    }
    
    [BurstCompile]
    public partial struct CharacterCollisionJob : IJobEntity 
    {
        [ReadOnly] public ComponentLookup<TileSensor> Sensors;
        
        private void Execute(ref LocalTransform transform, in CharacterSensors sensor)
        {
            TileSensor firstSensor = Sensors[sensor.First];
            TileSensor secondSensor = Sensors[sensor.Second];
            
            TileSensor targetSensor = firstSensor.Distance <= secondSensor.Distance ? firstSensor : secondSensor;
            
            transform.Position.xy += targetSensor.OffsetVector;
        }
    }
}
