using DnaCore.Character.Components;
using DnaCore.Character.Input;
using DnaCore.PhysicsEcs2D.Components;
using DnaCore.PhysicsEcs2D.Systems;
using DnaCore.PhysicsEcs2D.Tiles.Collision;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace DnaCore.Character.Systems
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
                    jump.CoyoteTime -= TimeSystem.Speed;
                    if (jump.CoyoteTime <= 0f) return;
                    break;
                }
                case Behaviours.Ground:
                {
                    jump.CoyoteTime = Jump.DefaultCoyoteTime; 
                    break;
                }
            }

            if (!input.Press.Jump) return;
            PerformJump(ref behaviour, ref jump, ref velocity, rotation);
        }

        private static void PerformJump(
            ref BehaviourTree behaviour, ref Jump jump, ref Velocity velocity, 
            in Rotation rotation)
        {
            jump.CoyoteTime = 0f;
            behaviour.Current = Behaviours.Air;
            float radians = math.radians(rotation.Angle);
            float2 jumpVector = jump.Speed * new float2(math.sin(radians), math.cos(radians));

            float2 trueValue = velocity.Vector + jumpVector;
            bool2 test = math.abs(jumpVector) < math.abs(trueValue);
            velocity.Vector = math.select(jumpVector, trueValue, test);
        }
    }
}
