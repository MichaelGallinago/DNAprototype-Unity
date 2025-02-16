using DnaCore.PhysicsEcs2D.DeltaTimeHelpers;
using Unity.Entities;

namespace DnaCore.PhysicsEcs2D.Components
{
    public struct Velocity : IComponentData
    {
        public AcceleratedVector2 Vector;
    }
}
