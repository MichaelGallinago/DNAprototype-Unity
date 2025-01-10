using Unity.Entities;

namespace Character.Components
{
    public struct Jump : IComponentData
    {
        public const float DefaultCoyoteTime = 30f;
        
        public float CoyoteTime;
    }
}
