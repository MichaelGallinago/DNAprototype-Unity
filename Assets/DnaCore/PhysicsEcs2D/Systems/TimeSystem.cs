using Scenes.Bootstrap;
using Unity.Burst;
using Unity.Entities;

namespace DnaCore.PhysicsEcs2D.Systems
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup), OrderFirst = true)]
    public partial class TimeSystem : SystemBase
    {
        public static float Speed => UnityEngine.Time.deltaTime * SimulationFrameRate.Data;
        
        private abstract class SimulationFrameRateKey {}
        private static readonly SharedStatic<float> SimulationFrameRate = 
            SharedStatic<float>.GetOrCreate<TimeSystem, SimulationFrameRateKey>();
        
        protected override void OnCreate()
        {
            SimulationFrameRate.Data = Settings.Options.SimulationRate;
            
            var systemGroup = World.GetExistingSystemManaged<FixedStepSimulationSystemGroup>();
            systemGroup.Timestep = 1f / Settings.Options.SimulationRate;
        }

        protected override void OnUpdate() {}
    }
}
