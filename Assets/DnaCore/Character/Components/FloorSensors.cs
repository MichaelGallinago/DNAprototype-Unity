using Unity.Entities;

namespace Character.Components
{
    public struct FloorSensors : IComponentData
    {
        public Entity First;
        public Entity Second;
    }
}
