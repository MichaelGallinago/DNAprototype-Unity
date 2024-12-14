using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Character.Input
{
    public struct PlayerInput : IComponentData
    {
        public Button Up;
        public Button Down;
        public Button Left;
        public Button Right;
        public Button Jump;
        public Button Sprint;
        public Button Attack;
        public Button Interact;
        public Button Crouch;
        public Button Pause;
        public float2 LookVector;
    }
    
    [BurstCompile]
    public struct Button
    {
        public bool Down;
        public bool Press;
        
        public void Set(bool value)
        {
            Press = value && !Down;
            Down = value;
        }
    }
}
