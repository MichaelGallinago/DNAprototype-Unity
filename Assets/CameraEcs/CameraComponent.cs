using Unity.Entities;
using Unity.Mathematics;

namespace CameraEcs
{
    public struct CameraComponent : IComponentData
    {
        public float2 Position;
        public Entity Target;
    }
}
