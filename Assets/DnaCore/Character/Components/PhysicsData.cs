using Unity.Entities;
using Unity.Mathematics;

namespace DnaCore.Character.Components
{
    public struct PhysicsData : IComponentData
    {
        public float Friction;
        public float Deceleration;
        public float Acceleration;
        public float AccelerationAir;
        public float AccelerationTop;
        public float2 VelocityCap;
    }
}
