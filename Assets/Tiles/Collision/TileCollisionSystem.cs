using Tiles.Generators;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace Tiles.Collision
{
    [BurstCompile]
    public partial struct TileCollisionSystem : ISystem, ISystemStartStop
    {
        private BlobAssetReference<TilesBlob> _tilesBlob;
        private NativeTilemap _tilemap;

        public void OnCreate(ref SystemState state)
        {
            if (!BlobAssetReference<TilesBlob>.TryRead(TileConstants.BlobPath, 0, out _tilesBlob))
            {
                Debug.LogError(nameof(TilesBlob) + " not loaded");
            }

            if (!SystemAPI.TryGetSingleton(out _tilemap))
            {
                Debug.LogError("No " + nameof(NativeTilemap) + " found");
            }
            
            state.RequireForUpdate<NativeTilemap>();
        }
        
        public void OnUpdate(ref SystemState state)
        {
            
        }
        
        public void OnDestroy(ref SystemState state)
        {
            if (_tilesBlob.IsCreated)
            {
                _tilesBlob.Dispose();
            }
        }

        public void OnStartRunning(ref SystemState state)
        {
            //throw new System.NotImplementedException();
        }

        public void OnStopRunning(ref SystemState state)
        {
            //throw new System.NotImplementedException();
        }
    }
}
