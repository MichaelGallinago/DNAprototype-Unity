using Unity.Entities;
using Unity.Mathematics;

namespace Character
{
    public struct Gravity : IComponentData
    {
        public float2 Vector;
    }
}
