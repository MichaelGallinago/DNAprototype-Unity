using Character.Components;
using Unity.Entities;

namespace Character
{
    public readonly partial struct BehaviourAspect : IAspect
    {
        private readonly RefRW<BehaviourTree> _behaviourTree;
        
        public bool IsChanged
        {
            get => _behaviourTree.ValueRO.IsChanged;
            set => _behaviourTree.ValueRW.IsChanged = value;
        }

        public Behaviours Current
        {
            get => _behaviourTree.ValueRO.Current;
            set =>  _behaviourTree.ValueRW.Current = value;
        }
        
        public Behaviours Previous => _behaviourTree.ValueRO.Previous;
    }
}
