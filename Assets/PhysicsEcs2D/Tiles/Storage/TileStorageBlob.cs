using PhysicsEcs2D.Tiles.Collision;
using Unity.Entities;

namespace PhysicsEcs2D.Tiles.Storage
{
    public struct TileStorageBlob
    {
        public BlobAssetReference<TileStorageBlobData> UniqueTilesReference;
    }

    public struct TileStorageBlobData
    {
        public BlobArray<NativeTile> UniqueTiles;
    }
}
