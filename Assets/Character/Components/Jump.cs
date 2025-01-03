using Unity.Entities;

namespace Character.Components
{
    public struct Jump : IComponentData, IEnableableComponent
    {
        public const float DefaultCoyoteTime = 30f;
        
        public float CoyoteTime;
    }
}
