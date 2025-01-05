using PhysicsEcs2D.DeltaTimeHelpers;
using Unity.Burst;
using Unity.Entities;

namespace Character
{
    [BurstCompile]
    public readonly partial struct GroundBehaviourAspect : IAspect
    {
        private readonly GroundSpeedAspect _groundSpeed;

        public AcceleratedValue Speed
        {
            get => _groundSpeed.Value;
            set => _groundSpeed.Value = value;
        }

        public bool IsEnabled
        {
            set => _groundSpeed.IsEnabled = value;
        }
    }
}
