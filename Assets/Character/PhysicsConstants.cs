using Unity.Mathematics;

namespace Character
{
    public static class PhysicsConstants
    {
        public const float Gravity = -0.21875f;
        public static readonly float2 GravityVector = new(0f, Gravity);
    }
}
