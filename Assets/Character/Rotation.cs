using Unity.Entities;
using Utilities;

namespace Character
{
    public struct Rotation : IComponentData
    {
        public float VisualAngle;
        public Quadrant Quadrant;
    }
}