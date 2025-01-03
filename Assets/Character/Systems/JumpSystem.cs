using Character.Components;
using Character.Input;
using PhysicsEcs2D;
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
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Jump>();
        }

        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new JumpJob().Schedule(state.Dependency);
        }
        
        public void OnDestroy(ref SystemState state) {}
    }

    [BurstCompile]
    [WithOptions(EntityQueryOptions.IgnoreComponentEnabledState)]
    public partial struct JumpJob : IJobEntity
    {
        private static void Execute(CharacterAspect character, JumpAspect jump, in PlayerInput input)
        {
            switch (character.Behaviour.Current)
            {
                case Behaviours.Air:
                {
                    jump.CoyoteTime -= Constants.Speed;
                    if (jump.CoyoteTime <= 0f)
                    {
                        jump.IsEnabled = false;
                        return;
                    }
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
                jump.IsEnabled = false;
                character.Behaviour.Current = Behaviours.Air;
                float radians = math.radians(character.GroundSpeed.Angle);
                var direction = new float2(math.sin(radians), math.cos(radians));
                character.Velocity += Constants.JumpSpeed * direction;
            }
        }
    }
}