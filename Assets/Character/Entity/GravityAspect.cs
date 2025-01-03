using PhysicsEcs2D.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Character
{
    [BurstCompile]
    public readonly partial struct GravityAspect : IAspect
    {
        private readonly RefRW<Gravity> _gravity;
        private readonly EnabledRefRW<Gravity> _isEnabled;
        
        public float2 Vector
        {
            get => _gravity.ValueRO.Vector;
            set => _gravity.ValueRW.Vector = value;
        }
        
        public bool IsEnabled
        {
            get => _isEnabled.ValueRO;
            set => _isEnabled.ValueRW = value;
        }
    }
}
