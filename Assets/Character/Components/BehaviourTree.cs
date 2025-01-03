using Unity.Burst;
using Unity.Entities;

namespace Character.Components
{
    [BurstCompile]
    public struct BehaviourTree : IComponentData
    {
        public bool IsChanged;

        public Behaviours Behaviour
        {
            get => _behaviour;
            set
            {
                if (_behaviour == value) return;
                IsChanged = true;
                PreviousBehaviour = _behaviour;
                _behaviour = value;
            }
        }
        private Behaviours _behaviour;
        
        public Behaviours PreviousBehaviour { get; private set; }
    }
    
    public enum Behaviours : byte { None, Ground, Air }
}