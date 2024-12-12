using BlobHashMaps;
using Unity.Entities;
using Unity.Mathematics;

namespace Tiles.Collision
{
    [MayOnlyLiveInBlobStorage]
    public struct TilemapBlob : IComponentData
    {
        public BlobAssetReference<BlobHashMap<int2, int>> BlobAssetTilemap;
    }
}
