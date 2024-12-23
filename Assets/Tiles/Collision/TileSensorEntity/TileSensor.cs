using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Utilities;

namespace Tiles.Collision.TileSensorEntity
{
    [BurstCompile]
    public struct TileSensor : IComponentData
    {
        public Quadrant Quadrant;
        public int Distance;
        public float Angle;
        
        public bool IsInside => Distance < 0;

        public float2 OffsetVector => Quadrant switch
        {
            Quadrant.Down => new float2(0f, -Distance),
            Quadrant.Right => new float2(Distance, 0f),
            Quadrant.Up => new float2(0f, Distance),
            Quadrant.Left => new float2(-Distance, 0f),
            _ => float2.zero
        };
        
        public readonly float2 AddOffset(float2 position) => Quadrant switch
        {
            Quadrant.Down => new float2(position.x, (int)position.y - Distance),
            Quadrant.Right => new float2((int)position.x + Distance, position.y),
            Quadrant.Up => new float2(0f, (int)position.y + Distance),
            Quadrant.Left => new float2((int)position.x - Distance, 0f),
            _ => position
        };

        public float2 EjectionVector => IsInside ? OffsetVector : float2.zero;
    }
}
