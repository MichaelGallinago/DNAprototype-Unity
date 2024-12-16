using Tiles.Collision;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Utilities;

namespace Character.Input
{
    [BurstCompile]
    [UpdateAfter(typeof(TileCollisionSystem))]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial struct PhysicsSystem : ISystem, ISystemStartStop
    {
        private EntityQuery _moveableQuery;
        private float _speed;
        
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
        }
        
        public void OnStartRunning(ref SystemState state) {}

        public void OnUpdate(ref SystemState state)
        {
            SetPhysicsSpeed(SystemAPI.Time.DeltaTime);
            
            new AccelerationJob { Speed = _speed }.ScheduleParallel();
            new GravityJob { Speed = _speed }.ScheduleParallel();
            new MovementJob { Speed = _speed }.ScheduleParallel();
        }

        public void OnStopRunning(ref SystemState state) {}
        
        public void OnDestroy(ref SystemState state) {}
        
        private void SetPhysicsSpeed(float deltaTime) => _speed = deltaTime * Storage.GameOptions.SimulationFrameRate;
    }
    
    [BurstCompile]
    public partial struct AccelerationJob : IJobEntity
    {
        public float Speed;
        
        private void Execute(ref Velocity velocity, in Acceleration gravity) => 
            velocity.Vector.AddAcceleration(gravity.Vector, Speed);
    }
    
    [BurstCompile]
    public partial struct GravityJob : IJobEntity
    {
        public float Speed;
        
        private void Execute(ref Velocity velocity, in Gravity gravity) => 
            velocity.Vector.AddAcceleration(gravity.Vector, Speed);
    }
    
    [BurstCompile]
    public partial struct MovementJob : IJobEntity
    {
        public float Speed;

        private void Execute(ref LocalTransform transform, ref Velocity velocity)
        {
            transform.Position.xy += velocity.Vector.GetValueDelta(Speed);
            velocity.Vector.ResetInstanceValue();
        }
    }
}
