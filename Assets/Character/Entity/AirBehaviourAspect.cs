using Character.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Character
{
    [BurstCompile]
    public readonly partial struct AirBehaviourAspect : IAspect
    {
        private readonly GravityAspect _gravity;
        private readonly RefRW<AirLock> _airLock;
        
        public float2 Gravity
        {
            get => _gravity.Vector;
            set => _gravity.Vector = value;
        }

        public bool IsLocked
        {
            get => _airLock.ValueRO.IsLocked;
            set => _airLock.ValueRW.IsLocked = value;
        }

        public bool IsEnabled
        {
            set => _gravity.IsEnabled = value;
        }
    }
}
