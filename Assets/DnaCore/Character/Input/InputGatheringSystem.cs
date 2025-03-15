using Unity.Burst;
using Unity.Entities;

namespace DnaCore.Character.Input
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup), OrderFirst = true)]
    public partial class InputGatheringSystem : SystemBase
    {
        private readonly IPlayerInputSource _source = new PlayerInputSource();

        protected override void OnCreate() => RequireForUpdate<PlayerInput>();
        protected override void OnStartRunning() => _source.Enable();
        protected override void OnStopRunning() => _source.Disable();
        protected override void OnUpdate() =>
            Dependency = new SetFixedInputSystem { Input = _source.FixedInput }.ScheduleParallel(Dependency);
    }

    [BurstCompile]
    public partial struct SetFixedInputSystem : IJobEntity
    {
        public PlayerInput Input;

        private readonly void Execute(ref PlayerInput input) => input = Input;
    }
}
