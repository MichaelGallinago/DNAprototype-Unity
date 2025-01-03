using Unity.Entities;

namespace PhysicsEcs2D.Tiles.Collision
{
    public struct TilesBlob
    {
        public BlobArray<NativeTile> Tiles;
    }
}
