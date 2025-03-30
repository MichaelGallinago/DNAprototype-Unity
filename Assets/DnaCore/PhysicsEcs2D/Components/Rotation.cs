using DnaCore.Utilities.Mathematics;
using Unity.Burst;
using Unity.Entities;

namespace DnaCore.PhysicsEcs2D.Components
{
    [BurstCompile]
    public struct Rotation : IComponentData
    {
        public float Radians;
        public float TurnAngle;
        public Direction Facing;
    }
}
