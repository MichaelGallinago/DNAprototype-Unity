using PhysicsEcs2D.DeltaTimeHelpers;
using Unity.Entities;

namespace Character.Components
{
    public struct GroundSpeed : IComponentData, IEnableableComponent
    {
        public AcceleratedValue Value;
        public float Angle;
    }
}
