using Unity.Entities;

namespace DnaCore.Character.Components
{
    public struct CharacterSensors : IComponentData
    {
        public Entity FloorLeft;
        public Entity FloorRight;
        //public Entity WallTop;
        //public Entity WallBottom;
    }
}
