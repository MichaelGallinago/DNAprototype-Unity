using Unity.Entities;

namespace Character
{
    public struct CharacterSensors : IComponentData
    {
        public Entity First;
        public Entity Second;
    }
}
