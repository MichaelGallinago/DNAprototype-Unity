using DnaCore.PhysicsEcs2D.DeltaTimeHelpers;
using Unity.Entities;

namespace DnaCore.Character.Components
{
    public struct GroundSpeed : IComponentData
    {
        public AcceleratedValue Value;
    }
}
