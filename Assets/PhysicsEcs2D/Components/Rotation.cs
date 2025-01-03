using Unity.Entities;
using Utilities;

namespace PhysicsEcs2D.Components
{
    public struct Rotation : IComponentData
    {
        public float VisualAngle;
        public Quadrant Quadrant;
    }
}