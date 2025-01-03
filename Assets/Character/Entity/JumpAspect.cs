using Character.Components;
using Unity.Burst;
using Unity.Entities;

namespace Character
{
    [BurstCompile]
    public readonly partial struct JumpAspect : IAspect
    {
        private readonly RefRW<Jump> _jump;
        private readonly EnabledRefRW<Jump> _isEnabled;
        
        public float CoyoteTime 
        {
            get => _jump.ValueRO.CoyoteTime;
            set => _jump.ValueRW.CoyoteTime = value;
        }
        
        public bool IsEnabled 
        { 
            get => _isEnabled.ValueRO; 
            set => _isEnabled.ValueRW = value; 
        }
    }
}
