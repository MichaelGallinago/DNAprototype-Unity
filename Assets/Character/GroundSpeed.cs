using Unity.Entities;

namespace Character
{
    public struct GroundSpeed : IComponentData
    {
        public AcceleratedValue Value;
        public float Angle;
    }
}
