using BlobHashMaps;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tiles.Generators
{
    [RequireComponent(typeof(Tilemap))]
    [RequireComponent(typeof(TilemapRenderer))]
    public class TilemapAuthoring : MonoBehaviour
    {
        [field: SerializeField] public Tilemap Tilemap { get; private set; }
        [field: SerializeField] public TilemapRenderer TilemapRenderer { get; private set; }
        
        public TileShape[] TileShapes => GetComponentsInChildren<TileShape>();
        
        private class TilemapBaker : Baker<TilemapAuthoring>
        {
            public override void Bake(TilemapAuthoring authoring)
            {
                DependsOn(authoring.Tilemap);
                
                var builder = new BlobBuilder(Allocator.Temp);
                NativeParallelHashMap<int2, int> source = GetTilePositions(authoring.Tilemap);
                builder.ConstructHashMap(ref builder.ConstructRoot<BlobHashMap<int2, int>>(), ref source);
                source.Dispose();
                
                AddComponent(GetEntity(TransformUsageFlags.None), new NativeTilemap
                {
                    IndexesReference = builder.CreateBlobAssetReference<BlobHashMap<int2, int>>(Allocator.Persistent)
                });
                
                builder.Dispose();
            }
            
            private static NativeParallelHashMap<int2, int> GetTilePositions(Tilemap tilemap)
            {
                BoundsInt bounds = tilemap.cellBounds;
                TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
                var tilePositions = new NativeParallelHashMap<int2, int>(allTiles.Length, Allocator.Temp);
            
                Vector3Int size = bounds.size;
                var index = 0;
                for (var y = 0; y < size.y; y++)
                for (var x = 0; x < size.x; x++)
                {
                    if (allTiles[index++] is not GeneratedTile tile) continue;
                    tilePositions.Add(new int2(x, y), tile.Index);
                }
                
                return tilePositions;
            }
        }
    }
    
    public struct NativeTilemap : IComponentData
    {
        public BlobAssetReference<BlobHashMap<int2, int>> IndexesReference;
    }
}
