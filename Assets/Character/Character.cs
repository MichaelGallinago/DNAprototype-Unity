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

        public bool IsGrounded
        {
            get => _behaviourTree.ValueRO.IsGrounded;
            set => _behaviourTree.ValueRW.IsGrounded = value;
        }

        public FloorSensors FloorSensor => _sensors.ValueRO;

        public ref AcceleratedVector2 Velocity => ref _velocity.ValueRW.Vector;
        
        public struct FloorSensors : IComponentData
        {
            public Entity First;
            public Entity Second;
        }
        
        public struct BehaviourTree : IComponentData
        {
            public bool IsGrounded;
        }
    }
}
