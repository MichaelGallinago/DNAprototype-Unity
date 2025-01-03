using Unity.Mathematics;
using Utilities;

namespace PhysicsEcs2D
{
    public static class Constants
    {
        public static float Speed => Storage.GameOptions.SimulationSpeed;
        
        public const float Gravity = -0.21875f;
        public const float JumpSpeed = 6.5f;
        public static readonly float2 GravityVector = new(0f, Gravity);
    }
}
