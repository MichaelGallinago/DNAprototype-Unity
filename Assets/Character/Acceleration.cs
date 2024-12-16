using Unity.Entities;
using Unity.Mathematics;

namespace Character
{
    public struct Acceleration : IComponentData
    {
        public float2 Vector;
    }
}
