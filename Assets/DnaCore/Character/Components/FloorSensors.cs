using Unity.Entities;

namespace DnaCore.Character.Components
{
    public struct FloorSensors : IComponentData
    {
        public Entity First;
        public Entity Second;
    }
}
