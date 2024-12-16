using Tiles.Models;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Tiles.Collision
{
    [BurstCompile]
    public struct NativeTile
    {
        public BlobArray<byte> HeightsDown;
        public BlobArray<byte> WidthsRight;
        public BlobArray<byte> HeightsUp;
        public BlobArray<byte> WidthsLeft;
        public float4 Angles;
        
        public byte GetSize(Quadrant quadrant, int2 position) => quadrant switch
        {
            Quadrant.Down => HeightsDown[position.x],
            Quadrant.Right => WidthsRight[position.y],
            Quadrant.Up => HeightsUp[position.x],
            Quadrant.Left => WidthsLeft[position.y],
            _ => default
        };

        public float GetAngle(Quadrant quadrant) => quadrant switch
        {
            Quadrant.Down => Angles.x,
            Quadrant.Right => Angles.y,
            Quadrant.Up => Angles.z,
            Quadrant.Left => Angles.w,
            _ => float.NaN
        };
    }
}
