using Character.Components;
using Character.Input;
using PhysicsEcs2D;
using PhysicsEcs2D.Components;
using PhysicsEcs2D.DeltaTimeHelpers;
using PhysicsEcs2D.Tiles.Collision;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Character.Systems
{
    [BurstCompile]
    [UpdateAfter(typeof(TileSenseSystem))]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial struct JumpSystem : ISystem
    {
        public void OnCreate(ref SystemState state) => 
            state.RequireForUpdate<Jump>();

        public void OnUpdate(ref SystemState state) =>
            state.Dependency = new JumpJob().ScheduleParallel(state.Dependency);
        
        public void OnDestroy(ref SystemState state) {}
    }

    [BurstCompile]
    public partial struct JumpJob : IJobEntity
    {
        private static void Execute(
            ref BehaviourTree behaviour, ref Jump jump, ref Velocity velocity,
            in Rotation rotation, in PlayerInput input)
        {
            switch (behaviour.Current)
            {
                case Behaviours.Air:
                {
                    jump.CoyoteTime -= Constants.Speed;
                    if (jump.CoyoteTime <= 0f) return;
                    break;
                }
                case Behaviours.Ground:
                {
                    jump.CoyoteTime = Jump.DefaultCoyoteTime; 
                    break;
                }
            }

            if (input.Press.Jump)
            {
                jump.CoyoteTime = 0f;
                behaviour.Current = Behaviours.Air;
                float radians = math.radians(rotation.Angle);
                var direction = new float2(math.sin(radians), math.cos(radians));
                velocity.Vector = Constants.JumpSpeed * direction;
            }
        }
    }
}
