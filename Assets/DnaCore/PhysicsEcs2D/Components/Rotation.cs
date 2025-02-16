using Unity.Burst;
using Unity.Entities;
using Utilities;

namespace DnaCore.PhysicsEcs2D.Components
{
    [BurstCompile]
    public struct Rotation : IComponentData
    {
        public float Angle;
        public float TurnAngle;
        public Quadrant Quadrant;
        public Direction Facing;
    }
}
