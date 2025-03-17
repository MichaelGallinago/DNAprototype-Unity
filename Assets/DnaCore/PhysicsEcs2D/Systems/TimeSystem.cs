using DnaCore.Settings;
using Unity.Burst;
using Unity.Entities;

namespace DnaCore.PhysicsEcs2D.Systems
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup), OrderFirst = true)]
    public partial class TimeSystem : SystemBase
    {
        public static float Speed => SimulationSpeed.Data;
        
        private abstract class SimulationFrameRateKey {}
        private static readonly SharedStatic<float> SimulationSpeed =
            SharedStatic<float>.GetOrCreate<TimeSystem, SimulationFrameRateKey>();
        
        protected override void OnCreate() =>
            World.GetExistingSystemManaged<FixedStepSimulationSystemGroup>().Timestep =
                1f / AppSettings.Options.SimulationRate;


        protected override void OnUpdate() =>
            SimulationSpeed.Data = Options.MinSimulationRate * SystemAPI.Time.DeltaTime;
    }
}
