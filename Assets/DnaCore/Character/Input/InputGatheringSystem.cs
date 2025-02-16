using Unity.Entities;

namespace DnaCore.Character.Input
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial class InputGatheringSystem : SystemBase
    {
        private readonly IPlayerInputSource _source = new PlayerInputSource();

        protected override void OnCreate() => RequireForUpdate<PlayerInput>();
        protected override void OnStartRunning() => _source.Enable();
        protected override void OnStopRunning() => _source.Disable();
        protected override void OnUpdate() => SystemAPI.SetSingleton(_source.FixedInput);
    }
}
