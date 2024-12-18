using Character.Input;
using Tiles.Collision.TileSensorEntity;
using Unity.Burst;
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
            new CharacterCollisionJob { EntityManager = state.EntityManager }.Schedule(state.Dependency);
            //state.Dependency = JobHandle.CombineDependencies(state.Dependency, state.GetJobHandle<PhysicsSystem>());
        }
        
        public void OnDestroy(ref SystemState state) {}
    }
    
    [BurstCompile]
    public partial struct CharacterCollisionJob : IJobEntity 
    {
        public EntityManager EntityManager;
        
        private void Execute(ref LocalTransform transform, in CharacterSensors sensor)
        {
            var firstSensor = EntityManager.GetComponentData<TileSensor>(sensor.First);
            var secondSensor = EntityManager.GetComponentData<TileSensor>(sensor.Second);
            
            TileSensor targetSensor = firstSensor.Distance <= secondSensor.Distance ? firstSensor : secondSensor;
            
            transform.Position.xy += targetSensor.OffsetVector;
        }
    }
}
