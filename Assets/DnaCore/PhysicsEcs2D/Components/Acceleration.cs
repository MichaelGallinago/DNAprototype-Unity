using Unity.Entities;
using Unity.Mathematics;

namespace PhysicsEcs2D.Components
{
    public struct Acceleration : IComponentData
    {
        public float2 Vector;
    }
}
