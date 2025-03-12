using Unity.Burst;
using Unity.Entities;

namespace DnaCore.Character.Components
{
    [BurstCompile]
    public struct BehaviourTree : IComponentData
    {
        public bool IsChanged;

        public Behaviours Current
        {
            get => _current;
            set
            {
                if (_current == value) return;
                IsChanged = true;
                Previous = _current;
                _current = value;
            }
        }
        private Behaviours _current;
        
        public Behaviours Previous { get; private set; }
    }
    
    public enum Behaviours : byte { None, Ground, Air }
}