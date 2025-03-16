using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace DnaCore.Character.Input
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup), OrderFirst = true)]
    public partial class InputGatheringSystem : SystemBase
    {
        private readonly IPlayerInputSource _source = new PlayerInputSource();

        protected override void OnCreate() => RequireForUpdate<CharacterInput>();
        protected override void OnStartRunning() => _source.Enable();
        protected override void OnStopRunning() => _source.Disable();
        protected override void OnUpdate() =>
            Dependency = new SetFixedInputSystem { Input = _source.FixedInput }.ScheduleParallel(Dependency);
    }

    [BurstCompile]
    public partial struct SetFixedInputSystem : IJobEntity
    {
        [ReadOnly] public CharacterInput Input;

        private readonly void Execute(ref CharacterInput input) => input = Input;
    }
}
