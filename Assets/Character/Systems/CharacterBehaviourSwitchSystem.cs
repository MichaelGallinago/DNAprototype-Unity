using Character.Components;
using PhysicsEcs2D.Components;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

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
            Debug.Log("Switching");
            if (!character.Behaviour.IsChanged) return;
            character.Behaviour.IsChanged = false;

            switch (character.Behaviour.Previous)
            {
                case Behaviours.Air: character.Gravity.IsEnabled = false; break;
                case Behaviours.Ground: character.GroundSpeed.IsEnabled = false; break;
            }
            
            switch (character.Behaviour.Current)
            {
                case Behaviours.Air: character.Gravity.IsEnabled = true; break;
                case Behaviours.Ground: character.GroundSpeed.IsEnabled = true; break;
            }
        }
    }
}
