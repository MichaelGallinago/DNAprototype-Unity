using Unity.Entities;
using Unity.Mathematics;

namespace DnaCore.PhysicsEcs2D.Components
{
    public struct Acceleration : IComponentData
    {
        public float2 Vector;
    }
}
