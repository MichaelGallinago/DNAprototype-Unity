using Unity.Entities;
using Unity.Mathematics;

namespace DnaCore.CameraEcs
{
    public struct CameraComponent : IComponentData
    {
        public float2 Position;
        public Entity Target;
    }
}
