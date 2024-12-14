using Unity.Entities;
using Unity.Mathematics;

namespace Character
{
    public struct Velocity : IComponentData
    {
        public float2 Value;
    }
}
