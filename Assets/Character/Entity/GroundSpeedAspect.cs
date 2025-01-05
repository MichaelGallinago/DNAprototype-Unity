using Character.Components;
using PhysicsEcs2D.DeltaTimeHelpers;
using Unity.Burst;
using Unity.Entities;

namespace Character
{
    [BurstCompile]
    public readonly partial struct GroundSpeedAspect : IAspect
    {
        private readonly RefRW<GroundSpeed> _gravity;
        private readonly EnabledRefRW<GroundSpeed> _isEnabled;
        
        public AcceleratedValue Value
        {
            get => _gravity.ValueRO.Value;
            set => _gravity.ValueRW.Value = value;
        }
        
        public bool IsEnabled
        {
            get => _isEnabled.ValueRO;
            set => _isEnabled.ValueRW = value;
        }
    }
}
