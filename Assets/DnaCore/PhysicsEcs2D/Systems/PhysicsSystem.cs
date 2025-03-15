using System.Diagnostics.CodeAnalysis;
using DnaCore.Character.Components;
using DnaCore.PhysicsEcs2D.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace DnaCore.PhysicsEcs2D.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial struct PhysicsSystem : ISystem
    {
        private EntityQuery _moveableQuery;
        
        public readonly void OnCreate(ref SystemState state) =>
            state.RequireForUpdate<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();

        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new AccelerationJob().ScheduleParallel(state.Dependency);
            state.Dependency = new GravityJob().ScheduleParallel(state.Dependency);
            state.Dependency = new MovementJob().ScheduleParallel(state.Dependency);
            state.Dependency.Complete();

            UpdateTransformSystemGroup();
        }
        
        public readonly void OnDestroy(ref SystemState state) {}
        
        private readonly void UpdateTransformSystemGroup()
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
            velocity.Vector.AddAcceleration(acceleration.Vector, TimeSystem.Speed);
    }
    
    [BurstCompile]
    [WithAll(typeof(AirTag))]
    public partial struct GravityJob : IJobEntity
    {
        private static void Execute(ref Velocity velocity, in Gravity gravity) =>
            velocity.Vector.AddAcceleration(gravity.Vector, TimeSystem.Speed);
    }
    
    [BurstCompile]
    public partial struct MovementJob : IJobEntity
    {
        private static void Execute(ref LocalTransform transform, ref Velocity velocity)
        {
            transform.Position.xy += velocity.Vector.GetValueDelta(TimeSystem.Speed);
            velocity.Vector.ResetInstanceValue();
        }
    }
}
