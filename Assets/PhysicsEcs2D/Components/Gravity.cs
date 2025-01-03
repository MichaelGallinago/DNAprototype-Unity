using Unity.Entities;
using Unity.Mathematics;

namespace PhysicsEcs2D.Components
{
    public struct Gravity : IComponentData, IEnableableComponent
    {
        public float2 Vector;
    }
}
