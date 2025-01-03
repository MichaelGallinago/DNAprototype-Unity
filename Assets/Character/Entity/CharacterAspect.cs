using PhysicsEcs2D.Components;
using PhysicsEcs2D.DeltaTimeHelpers;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Utilities;

namespace Character
{
    [BurstCompile]
    public readonly partial struct CharacterAspect : IAspect
    {
        public readonly Entity Entity;
        public readonly BehaviourAspect Behaviour;
        public readonly GroundSpeedAspect GroundSpeed;
        public readonly GravityAspect Gravity;
        
        private readonly RefRW<LocalTransform> _transform;
        private readonly RefRW<Velocity> _velocity;
        private readonly RefRW<Rotation> _rotation;
        
        private readonly RefRO<FloorSensors> _sensors;
        
        public float2 Position
        {
            get => _transform.ValueRO.Position.xy;
            set => _transform.ValueRW.Position.xy = value;
        }
        
        public Quadrant Quadrant
        {
            get => _rotation.ValueRO.Quadrant;
            set => _rotation.ValueRW.Quadrant = value;
        }
        
        public FloorSensors FloorSensor => _sensors.ValueRO;
        
        public ref AcceleratedVector2 Velocity => ref _velocity.ValueRW.Vector;
        
        public struct FloorSensors : IComponentData
        {
            public Entity First;
            public Entity Second;
        }
    }
}
