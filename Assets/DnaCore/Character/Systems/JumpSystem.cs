using DnaCore.Character.Components;
using DnaCore.Character.Input;
using DnaCore.PhysicsEcs2D.Components;
using DnaCore.PhysicsEcs2D.Systems;
using DnaCore.PhysicsEcs2D.Tiles.Collision;
using DnaCore.Utilities;
using DnaCore.Utilities.Mathematics;
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
            in Rotation rotation, in CharacterInput input)
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
            PerformJump(ref behaviour, ref jump, ref velocity, in rotation);
        }

        private static void PerformJump(
            ref BehaviourTree behaviour, ref Jump jump, ref Velocity velocity, 
            in Rotation rotation)
        {
            jump.CoyoteTime = 0f;
            behaviour.Current = Behaviours.Air;
            float2 jumpVector = jump.Speed * new float2(-math.sin(rotation.Radians), math.cos(rotation.Radians));

            bool isVerticalQuadrant = MathUtilities.GetQuadrant(rotation.Radians) is Quadrant.Down or Quadrant.Up;
            var isAdditiveJump = new bool2(isVerticalQuadrant, !isVerticalQuadrant);

            float2 additiveVector = velocity.Vector + jumpVector;
            isAdditiveJump |= math.abs(jumpVector) < math.abs(additiveVector);

            velocity.Vector = math.select(jumpVector, additiveVector, isAdditiveJump);
        }
    }
}
