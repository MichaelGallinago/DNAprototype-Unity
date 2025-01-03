using Character.Components;
using PhysicsEcs2D.DeltaTimeHelpers;
using Unity.Burst;
using Unity.Entities;

namespace Character
{
    [BurstCompile]
    public readonly partial struct GroundSpeedAspect : IAspect
    {
        private readonly RefRW<GroundSpeed> _groundSpeed;
        private readonly EnabledRefRW<GroundSpeed> _isEnabled;
        
        public AcceleratedValue Value
        {
            get => _groundSpeed.ValueRO.Value;
            set => _groundSpeed.ValueRW.Value = value;
        }
        
        public float Angle
        {
            get => _groundSpeed.ValueRO.Angle;
            set => _groundSpeed.ValueRW.Angle = value;
        }
        
        public bool IsEnabled
        {
            get => _isEnabled.ValueRO;
            set => _isEnabled.ValueRW = value;
        }
    }
}
