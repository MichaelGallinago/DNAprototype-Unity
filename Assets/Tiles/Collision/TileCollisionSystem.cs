using Tiles.Generators;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace Tiles.Collision
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
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
            
            state.RequireForUpdate<NativeTilemap>();
        }
        
        public void OnStartRunning(ref SystemState state)
        {
            if (!SystemAPI.TryGetSingleton(out _tilemap))
            {
                Debug.LogError("No " + nameof(NativeTilemap) + " found");
            }
            else
            {
                Debug.Log(_tilemap.IndexesReference.Value.Count);
            }
        }
        
        public void OnUpdate(ref SystemState state)
        {
            
        }
        
        public void OnStopRunning(ref SystemState state) {}
        
        public void OnDestroy(ref SystemState state)
        {
            if (_tilesBlob.IsCreated)
            {
                _tilesBlob.Dispose();
            }
        }
    }
}
