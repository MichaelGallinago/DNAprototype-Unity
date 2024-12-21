using Unity.Entities;
using Unity.Mathematics;

namespace Character
{
    public struct Gravity : IComponentData, IEnableableComponent
    {
        public float2 Vector;
    }
}
