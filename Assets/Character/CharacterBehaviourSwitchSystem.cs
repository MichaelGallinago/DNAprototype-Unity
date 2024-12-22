using Unity.Burst;
using Unity.Entities;

namespace Character
{
    [BurstCompile]
    [UpdateAfter(typeof(CharacterCollisionSystem))]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial struct CharacterBehaviourSwitchSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<Character.BehaviourTree>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var singleton = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
            EntityCommandBuffer ecb = singleton.CreateCommandBuffer(state.WorldUnmanaged);
            
            new CharacterBehaviourSwitchJob
            {
                Ecb = ecb.AsParallelWriter()
            }.Schedule(state.Dependency).Complete();
        }

        public void OnDestroy(ref SystemState state) {}
    }

    [BurstCompile]
    public partial struct CharacterBehaviourSwitchJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter Ecb;
        
        private void Execute(Character character, [ChunkIndexInQuery] int index)
        {
            if (!character.IsBehaviourChanged) return;
            character.IsBehaviourChanged = false;

            switch (character.PreviousBehaviour)
            {
                case Character.Behaviours.Airborne:
                    Ecb.SetComponentEnabled<Gravity>(index, character.Entity, false);
                    break;
            }
            
            switch (character.Behaviour)
            {
                case Character.Behaviours.Airborne:
                    Ecb.SetComponentEnabled<Gravity>(index, character.Entity, true);
                    break;
            }
        }
    }
}
