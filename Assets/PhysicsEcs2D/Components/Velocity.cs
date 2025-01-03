using PhysicsEcs2D.DeltaTimeHelpers;
using Unity.Entities;

namespace PhysicsEcs2D.Components
{
    public struct Velocity : IComponentData
    {
        public AcceleratedVector2 Vector;
    }
}
