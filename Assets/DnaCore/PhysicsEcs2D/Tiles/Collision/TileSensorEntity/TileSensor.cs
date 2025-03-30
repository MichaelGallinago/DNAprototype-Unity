using DnaCore.Utilities.Mathematics;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace DnaCore.PhysicsEcs2D.Tiles.Collision.TileSensorEntity
{
    [BurstCompile]
    public struct TileSensor : IComponentData
    {
        public int2 Position;
        public Quadrant Quadrant;
        public int Distance;
        public float Radians;
        
        public bool IsInside => Distance < 0;
        
        public readonly float2 AddOffset(float2 position) => Quadrant switch
        {
            Quadrant.Down => new float2(position.x, (int)position.y - Distance),
            Quadrant.Right => new float2((int)position.x + Distance, position.y),
            Quadrant.Up => new float2(position.x, (int)position.y + Distance),
            Quadrant.Left => new float2((int)position.x - Distance, position.y),
            _ => position
        };
    }
}
