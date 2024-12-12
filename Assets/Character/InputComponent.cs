using Unity.Entities;
using Unity.Mathematics;

namespace Character
{
    public struct Input : IComponentData
    {
        public float2 MoveDirection;
        public bool Jump;
    }
}
