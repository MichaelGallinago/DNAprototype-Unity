using Unity.Mathematics;
using Utilities;

namespace PhysicsEcs2D
{
    public static class Constants
    {
        public static float Speed => Storage.GameOptions.SimulationSpeed;
        
        public const float Gravity = -0.21875f;
        public const float JumpSpeed = 6.5f;
        public const float Friction = 0.046875f;
        public const float Deceleration = 0.5f;
        public const float Acceleration = 0.046875f;
        public const float AccelerationAir = 0.09375f;
        public const float AccelerationTop = 6f;
        public const float SpeedCap = 32f;
        public static readonly float2 VelocityCap = new(SpeedCap, SpeedCap);
        public static readonly float2 GravityVector = new(0f, Gravity);
    }
}
