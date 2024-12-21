using Unity.Entities;

namespace Character
{
    public struct GroundSpeed : IComponentData, IEnableableComponent
    {
        public AcceleratedValue Value;
        public float Angle;
    }
}
