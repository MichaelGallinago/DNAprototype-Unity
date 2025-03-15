using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace DnaCore.Character.Input
{
    public struct CharacterInput : IComponentData
    {
        public Buttons Down;
        public Buttons Press;
        public float2 LookVector;
    }

    [BurstCompile]
    public struct Buttons
    {
        public bool Up;
        public bool Down;
        public bool Left;
        public bool Right;
        public bool Jump;
        public bool Sprint;
        public bool Attack;
        public bool Interact;
        public bool Crouch;
        public bool Pause;

        public Buttons ApplyPressBuffer(Buttons pressBuffer)
        {
            Buttons copy = this;
            copy.Up &= !pressBuffer.Up;
            copy.Down &= !pressBuffer.Down;
            copy.Left &= !pressBuffer.Left;
            copy.Right &= !pressBuffer.Right;
            copy.Jump &= !pressBuffer.Jump;
            copy.Sprint &= !pressBuffer.Sprint;
            copy.Attack &= !pressBuffer.Attack;
            copy.Interact &= !pressBuffer.Interact;
            copy.Crouch &= !pressBuffer.Crouch;
            copy.Pause &= !pressBuffer.Pause;
            return copy;
        }
    }
}
