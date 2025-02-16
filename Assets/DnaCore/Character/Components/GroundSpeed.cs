using PhysicsEcs2D.DeltaTimeHelpers;
using Unity.Entities;

namespace Character.Components
{
    public struct GroundSpeed : IComponentData
    {
        public AcceleratedValue Value;
    }
}
