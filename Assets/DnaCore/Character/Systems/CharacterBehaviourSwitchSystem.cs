using DnaCore.Character.Components;
using Unity.Burst;
using Unity.Entities;

namespace DnaCore.Character.Systems
{
    [BurstCompile]
    [UpdateAfter(typeof(CharacterCollisionSystem))]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial struct CharacterBehaviourSwitchSystem : ISystem
    {
        public void OnCreate(ref SystemState state) =>
            state.RequireForUpdate<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();

        public void OnUpdate(ref SystemState state) =>
            state.Dependency = new CharacterBehaviourSwitchJob().Schedule(state.Dependency);

        public void OnDestroy(ref SystemState state) {}
    }

    [BurstCompile]
    [WithPresent(typeof(GroundTag), typeof(AirTag))]
    public partial struct CharacterBehaviourSwitchJob : IJobEntity
    {
        private static void Execute(
            ref BehaviourTree behaviour, 
            EnabledRefRW<GroundTag> groundTag, EnabledRefRW<AirTag> airTag)
        {
            if (!behaviour.IsChanged) return;
            behaviour.IsChanged = false;
            
            switch (behaviour.Previous)
            {
                case Behaviours.Air: airTag.ValueRW = false; break;
                case Behaviours.Ground: groundTag.ValueRW = false; break;
            }
            
            switch (behaviour.Current)
            {
                case Behaviours.Air: airTag.ValueRW = true; break;
                case Behaviours.Ground: groundTag.ValueRW = true; break;
            }
        }
    }
}
