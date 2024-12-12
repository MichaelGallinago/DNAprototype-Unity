using Tiles.Models;
using Unity.Entities;
using Unity.Mathematics;

namespace Tiles.Collision
{
    public struct TileBlob
    {
        public BlobArray<byte> HeightsDown;
        public BlobArray<byte> WidthsRight;
        public BlobArray<byte> HeightsUp;
        public BlobArray<byte> WidthsLeft;
        public float4 Angles;
        
        public byte GetSize(Quadrant quadrant, int x, int y) => quadrant switch
        {
            Quadrant.Down => HeightsDown[x],
            Quadrant.Right => WidthsRight[y],
            Quadrant.Up => HeightsUp[x],
            Quadrant.Left => WidthsLeft[y],
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
