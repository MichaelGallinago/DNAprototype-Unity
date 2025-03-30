using BlobHashMaps;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DnaCore.PhysicsEcs2D.Tiles.Generators.TilemapGenerator
{
    [ExecuteAlways]
    [RequireComponent(typeof(Tilemap))]
    [RequireComponent(typeof(TilemapRenderer))]
    public class TilemapAuthoring : MonoBehaviour
    {
        [field: SerializeField] public Tilemap Tilemap { get; private set; }
        [field: SerializeField] public TilemapRenderer TilemapRenderer { get; private set; }
        
        public TileShape[] TileShapes => GetComponentsInChildren<TileShape>();

        private void Start() => Tilemap.RefreshAllTiles(); // Color initialization fix (with [ExecuteAlways])

        private class Baker : Baker<TilemapAuthoring>
        {
            public override void Bake(TilemapAuthoring authoring)
            {
                DependsOn(authoring.Tilemap);

                NativeParallelHashMap<int2, int> source = GetTilePositions(authoring.Tilemap);
                if (!source.IsCreated || source.IsEmpty)
                {
                    source.Dispose();
                    return;
                }
                
                var builder = new BlobBuilder(Allocator.Temp);
                builder.ConstructHashMap(ref builder.ConstructRoot<BlobHashMap<int2, int>>(), ref source);
                source.Dispose();

                BlobAssetReference<BlobHashMap<int2, int>> blobReference =
                    builder.CreateBlobAssetReference<BlobHashMap<int2, int>>(Allocator.Persistent);
                AddBlobAsset(ref blobReference, out Unity.Entities.Hash128 _);

                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new NativeTilemap { IndexesReference = blobReference });

                if (IsBakingForEditor())
                {
                    AddComponentObject(entity, new TilemapRenderComponent
                    {
                        GameObject = authoring.TilemapRenderer.gameObject
                    });
                }
                
                builder.Dispose();
            }

            private static NativeParallelHashMap<int2, int> GetTilePositions(Tilemap tilemap)
            {
                BoundsInt bounds = tilemap.cellBounds;
                TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
                var tilePositions = new NativeParallelHashMap<int2, int>(allTiles.Length, Allocator.Temp);
                
                var index = 0;
                var boundsMax = new int2(bounds.xMax, bounds.yMax);
                for (int y = bounds.yMin; y < boundsMax.y; y++)
                for (int x = bounds.xMin; x < boundsMax.x; x++)
                {
                    if (allTiles[index++] is not GeneratedTile tile) continue;
                    tilePositions.Add(new int2(x, y), tile.Index);
                }

                return tilePositions;
            }
        }
    }
}
