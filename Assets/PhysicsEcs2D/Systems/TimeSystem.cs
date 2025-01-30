using Unity.Entities;

namespace PhysicsEcs2D.Systems
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial class TimeSystem : SystemBase
    {
        public static float Speed => UnityEngine.Time.deltaTime;
        
        protected override void OnCreate()
        {
            var systemGroup = World.GetExistingSystemManaged<FixedStepSimulationSystemGroup>();
            
            const float timeStep = 1f / 60f;
            systemGroup.Timestep = timeStep;
        }

        protected override void OnUpdate() {}
    }
}
