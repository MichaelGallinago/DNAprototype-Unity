using Unity.Entities;

namespace Character.Components
{
    public struct Jump : IComponentData
    {
        public const float DefaultCoyoteTime = 10f;
        
        public float CoyoteTime;
    }
}
