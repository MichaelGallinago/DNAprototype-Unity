using PhysicsEcs2D;
using PhysicsEcs2D.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Character.Input
{
    [BurstCompile]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial struct PhysicsSystem : ISystem, ISystemStartStop
    {
        private EntityQuery _moveableQuery;
        
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
        }
        
        public void OnStartRunning(ref SystemState state) {}

        public void OnUpdate(ref SystemState state)
        {
            //state.Dependency = new AccelerationJob().ScheduleParallel(state.Dependency);
            //state.Dependency = new GravityJob().ScheduleParallel(state.Dependency); 
            //state.Dependency = new MovementJob().ScheduleParallel(state.Dependency);
            state.Dependency.Complete();

            UpdateTransformSystemGroup();
        }

        public void OnStopRunning(ref SystemState state) {}
        
        public void OnDestroy(ref SystemState state) {}

        private void UpdateTransformSystemGroup()
        {
            var world = World.DefaultGameObjectInjectionWorld;
            var transformSystemGroup = world.GetOrCreateSystemManaged<TransformSystemGroup>();
            transformSystemGroup.Update();
        }
    }
    
    [BurstCompile]
    public partial struct AccelerationJob : IJobEntity
    {
        private static void Execute(ref Velocity velocity, in Acceleration acceleration) => 
            velocity.Vector.AddAcceleration(acceleration.Vector, Constants.Speed);
    }
    
    [BurstCompile]
    public partial struct GravityJob : IJobEntity
    {
        private static void Execute(ref Velocity velocity, in Gravity gravity) =>
            velocity.Vector.AddAcceleration(gravity.Vector, Constants.Speed);
    }
    
    [BurstCompile]
    public partial struct MovementJob : IJobEntity
    {
        private static void Execute(ref LocalTransform transform, ref Velocity velocity)
        {
            transform.Position.xy += velocity.Vector.GetValueDelta(Constants.Speed);
            velocity.Vector.ResetInstanceValue();
        }
    }
}
