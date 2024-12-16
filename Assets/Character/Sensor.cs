using Tiles.Models;
using Unity.Entities;
using Unity.Mathematics;

namespace Character
{
    public struct TileSensor : IComponentData
    {
        public Quadrant Quadrant;
        public int2 Offset;
        public int Distance;
        public float Angle;
    }
}
