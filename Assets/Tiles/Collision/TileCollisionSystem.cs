using Tiles.Generators;
using Unity.Entities;
using UnityEngine;

namespace Tiles.Collision
{
    public partial class AbasSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<NativeTilemap>();
        }

        protected override void OnUpdate()
        {
            //throw new System.NotImplementedException();
        }
    }
    
    public partial struct TileCollisionSystem : ISystem
    {
        private BlobAssetReference<TilesBlob> _tiles;

        public void OnCreate(ref SystemState state)
        {
            if (!BlobAssetReference<TilesBlob>.TryRead(TileConstants.BlobPath, 0, out _tiles))
            {
                Debug.LogError("Fuck");
            }
            
            Debug.Log(SystemAPI.TryGetSingleton(out NativeTilemap tilemap)
                ? tilemap.IndexesReference.Value.Count.ToString()
                : "No data found");
        }
        
        public void OnUpdate(ref SystemState state)
        {
            
        }
        
        public void OnDestroy(ref SystemState state)
        {
            if (_tiles.IsCreated)
            {
                _tiles.Dispose();
            }
        }
    }
}
