using Tiles.Models;
using Unity.Entities;

namespace Character.TileSensor
{
    public struct TileSensor : IComponentData
    {
        public Quadrant Quadrant;
        public int Distance;
        public float Angle;
    }
}
