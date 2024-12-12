using Tiles.Collision;
using Unity.Entities;

namespace Tiles.Storage
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
