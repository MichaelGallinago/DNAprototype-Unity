using Unity.Entities;

namespace Character
{
    public struct GroundSpeed : IComponentData
    {
        public float Value;
        public float Angle;
    }
}