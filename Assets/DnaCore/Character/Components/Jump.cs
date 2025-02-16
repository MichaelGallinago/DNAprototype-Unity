using Unity.Entities;

namespace DnaCore.Character.Components
{
    public struct Jump : IComponentData
    {
        public const float DefaultCoyoteTime = 10f;
        
        public float CoyoteTime;
        public float Speed;
    }
}
