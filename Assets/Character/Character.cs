using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Utilities;

namespace Character
{
    [BurstCompile]
    public readonly partial struct Character : IAspect
    {
        public readonly Entity Entity;
        
        private readonly RefRW<BehaviourTree> _behaviourTree;
        private readonly RefRW<LocalTransform> _transform;
        private readonly RefRW<GroundSpeed> _groundSpeed;
        private readonly RefRW<Velocity> _velocity;
        private readonly RefRW<Rotation> _rotation;
        
        private readonly RefRO<FloorSensors> _sensors;

        public float2 Position
        {
            get => _transform.ValueRO.Position.xy;
            set => _transform.ValueRW.Position.xy = value;
        }

        public float Angle
        {
            get => _groundSpeed.ValueRO.Angle;
            set => _groundSpeed.ValueRW.Angle = value;
        }
        
        public AcceleratedValue GroundSpeed
        {
            get => _groundSpeed.ValueRO.Value;
            set => _groundSpeed.ValueRW.Value = value;
        }
        
        public Quadrant Quadrant
        {
            get => _rotation.ValueRO.Quadrant;
            set => _rotation.ValueRW.Quadrant = value;
        }
        
        public bool IsBehaviourChanged
        {
            get => _behaviourTree.ValueRO.IsChanged;
            set => _behaviourTree.ValueRW.IsChanged = value;
        }

        public Behaviours Behaviour
        {
            get => _behaviourTree.ValueRO.Behaviour;
            set => _behaviourTree.ValueRW.Behaviour = value;
        }
        
        public Behaviours PreviousBehaviour => _behaviourTree.ValueRO.PreviousBehaviour;

        public FloorSensors FloorSensor => _sensors.ValueRO;

        public ref AcceleratedVector2 Velocity => ref _velocity.ValueRW.Vector;
        
        public struct FloorSensors : IComponentData
        {
            public Entity First;
            public Entity Second;
        }
        
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
                    _previousBehaviour = _behaviour;
                    _behaviour = value;
                }
            }
            private Behaviours _behaviour;
            
            public Behaviours PreviousBehaviour => _previousBehaviour;
            private Behaviours _previousBehaviour;
        }
        
        public enum Behaviours : byte { None, Ground, Airborne }
    }
}
