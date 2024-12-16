using Unity.Entities;

namespace Character
{
    public struct Velocity : IComponentData
    {
        public AcceleratedVector2 Vector;
    }
}
