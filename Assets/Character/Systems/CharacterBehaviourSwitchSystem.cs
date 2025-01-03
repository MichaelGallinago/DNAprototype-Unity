using Character.Components;
using PhysicsEcs2D.Components;
using Unity.Burst;
using Unity.Entities;

namespace Character.Systems
{
    [BurstCompile]
    [UpdateAfter(typeof(CharacterCollisionSystem))]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial struct CharacterBehaviourSwitchSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<BehaviourTree>();
        }

        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new CharacterBehaviourSwitchJob().Schedule(state.Dependency);
        }

        public void OnDestroy(ref SystemState state) {}
    }

    [BurstCompile]
    public partial struct CharacterBehaviourSwitchJob : IJobEntity
    {
        private void Execute(CharacterAspect character)
        {
            if (!character.Behaviour.IsChanged) return;
            character.Behaviour.IsChanged = false;

            switch (character.Behaviour.Previous)
            {
                //case Behaviours.Air: characterEnable.Gravity.IsEnabled = false; break;
                //case Behaviours.Ground: characterEnable.GroundSpeed.IsEnabled = false; break;
            }
            
            switch (character.Behaviour.Current)
            {
                //case Behaviours.Air: characterEnable.Gravity.IsEnabled = true; break;
                //case Behaviours.Ground: characterEnable.GroundSpeed.IsEnabled = true; break;
            }
        }
    }
}
