using PhysicsEcs2D.Components;
using Unity.Entities;
using Utilities;

namespace Character
{
    public readonly partial struct RotationAspect : IAspect
    {
        private readonly RefRW<Rotation> _rotation;
        
        public float Angle
        {
            get => _rotation.ValueRO.Angle;
            set => _rotation.ValueRW.Angle = value;
        }
        
        public float TurnAngle
        {
            get => _rotation.ValueRO.TurnAngle;
            set => _rotation.ValueRW.TurnAngle = value;
        }

        public Quadrant Quadrant
        {
            get => _rotation.ValueRO.Quadrant;
            set => _rotation.ValueRW.Quadrant = value;
        }
        
        public Direction Facing
        {
            get => _rotation.ValueRO.Facing;
            set => _rotation.ValueRW.Facing = value;
        }
    }
}
