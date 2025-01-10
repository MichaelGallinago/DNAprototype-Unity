using Character.Components;
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
        }

        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new CharacterBehaviourSwitchJob().Schedule(state.Dependency);
        }

        public void OnDestroy(ref SystemState state) {}
    }

    [BurstCompile]
    [WithPresent(typeof(GroundSpeed))]
    //[WithOptions(EntityQueryOptions.IgnoreComponentEnabledState)]
    public partial struct CharacterBehaviourSwitchJob : IJobEntity
    {
        private static void Execute(
            in BehaviourTree behaviour,
            ref AirLock airLock,
            //AirBehaviourAspect airBehaviour,
            GroundBehaviourAspect groundBehaviour)
        {
            //Debug.Log($"{(int)behaviour.Current}");
            //if (!behaviour.IsChanged) return;
            //behaviour.IsChanged = false;
            /*
            switch (behaviour.Previous)
            {
                //case Behaviours.Air: airBehaviour.IsEnabled = false; break;
                case Behaviours.Ground: groundBehaviour.IsEnabled = false; break;
            }
            
            switch (behaviour.Current)
            {
                //case Behaviours.Air: airBehaviour.IsEnabled = true; break;
                case Behaviours.Ground: groundBehaviour.IsEnabled = true; break;
            }
            */
        }
    }
}
