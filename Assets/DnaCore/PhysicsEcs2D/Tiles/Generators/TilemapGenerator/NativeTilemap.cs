using BlobHashMaps;
using Unity.Entities;
using Unity.Mathematics;

namespace DnaCore.PhysicsEcs2D.Tiles.Generators.TilemapGenerator
{
    public struct NativeTilemap : IComponentData
    {
        public BlobAssetReference<BlobHashMap<int2, int>> IndexesReference;
    }
}
