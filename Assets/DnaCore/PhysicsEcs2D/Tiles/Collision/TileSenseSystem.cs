using System.Diagnostics.CodeAnalysis;
using DnaCore.PhysicsEcs2D.Systems;
using DnaCore.PhysicsEcs2D.Tiles.Collision.TileSensorEntity;
using DnaCore.PhysicsEcs2D.Tiles.Generators;
using DnaCore.Utilities;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static DnaCore.PhysicsEcs2D.Tiles.TileConstants;

namespace DnaCore.PhysicsEcs2D.Tiles.Collision
{
    [BurstCompile]
    [UpdateAfter(typeof(PhysicsSystem))]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial struct TileSenseSystem : ISystem, ISystemStartStop
    {
        private BlobAssetReference<TilesBlob> _tilesBlob;
        private NativeTilemap _tilemap;

        public void OnCreate(ref SystemState state)
        {
            if (!BlobAssetReference<TilesBlob>.TryRead(BlobPath, 0, out _tilesBlob))
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
        }
        
        [SuppressMessage("Performance", "EPS12:A struct member can be made readonly")]
        public void OnUpdate(ref SystemState state)
        {
	        state.Dependency = new TileSenseJob
	        {
		        TilesBlob = _tilesBlob, 
		        Tilemap = _tilemap
	        }.ScheduleParallel(state.Dependency);
        }
        
        public readonly void OnStopRunning(ref SystemState state) {}
        
        public void OnDestroy(ref SystemState state)
        {
            if (_tilesBlob.IsCreated)
            {
                _tilesBlob.Dispose();
            }
        }
    }
    
    [BurstCompile]
    public partial struct TileSenseJob : IJobEntity
    {
	    private const int MaxDistance = Size * 2;
	    
	    [ReadOnly] public BlobAssetReference<TilesBlob> TilesBlob;
	    [ReadOnly] public NativeTilemap Tilemap;

        private void Execute(ref LocalToWorld transform, ref TileSensor sensor) =>
	        FindTileData((int2)transform.Position.xy, ref sensor);

        private void FindTileData(int2 targetPosition, ref TileSensor sensor)
        {
	        int2 inTilePosition = targetPosition & ModSize;
	        targetPosition >>= DivSize;

	        if (!TrySearch(targetPosition, 0, sensor.Quadrant, out int index))
	        {
		        FindFurtherTile(targetPosition, inTilePosition, ref sensor);
		        return;
	        }

	        ref NativeTile tile = ref GetTile(index);
	        byte size = tile.GetSize(sensor.Quadrant, inTilePosition);

	        switch (size)
	        {
		        case 0:
			        FindFurtherTile(targetPosition, inTilePosition, ref sensor);
			        break;
		        case Size:
			        sensor.Radians = tile.GetAngle(sensor.Quadrant);
			        FindCloserTile(targetPosition, inTilePosition, ref sensor);
			        break;
		        default:
			        sensor.Distance = CalculateInTilePosition(inTilePosition, sensor.Quadrant) - size;
			        sensor.Radians = tile.GetAngle(sensor.Quadrant);
			        break;
	        }
        }

        private void FindFurtherTile(int2 targetPosition, int2 inTilePosition, ref TileSensor sensor)
        {
	        if (TrySearch(targetPosition, 1, sensor.Quadrant, out int index))
	        {
		        ref NativeTile tile = ref GetTile(index);
		        byte size = tile.GetSize(sensor.Quadrant, inTilePosition);

		        if (size > 0)
		        {
			        sensor.Distance = CalculateInTilePosition(inTilePosition, sensor.Quadrant) - size + Size;
			        sensor.Radians = tile.GetAngle(sensor.Quadrant);
			        return;
		        }
	        }

	        sensor.Distance = MaxDistance;
	        sensor.Radians = float.NaN;
        }

        private void FindCloserTile(int2 targetPosition, int2 inTilePosition, ref TileSensor sensor)
        {
	        byte size;
	        if (TrySearch(targetPosition, -1, sensor.Quadrant, out int index))
	        {
		        ref NativeTile tile = ref GetTile(index);
		        size = tile.GetSize(sensor.Quadrant, inTilePosition);

		        if (size > 0)
		        {
			        sensor.Radians = tile.GetAngle(sensor.Quadrant);
		        }
	        }
	        else
	        {
		        size = 0;
	        }

	        sensor.Distance = CalculateInTilePosition(inTilePosition, sensor.Quadrant) - size - Size;
        }

        private static int CalculateInTilePosition(int2 inTilePosition, Quadrant quadrant) => quadrant switch
        {
	        Quadrant.Down => inTilePosition.y,
	        Quadrant.Right => ModSize - inTilePosition.x,
	        Quadrant.Up => ModSize - inTilePosition.y,
	        Quadrant.Left => inTilePosition.x,
	        _ => 0
        };
        
        private bool TrySearch(int2 position, sbyte shift, Quadrant quadrant, out int index)
        {
	        switch (quadrant)
	        {
		        case Quadrant.Down: position.y -= shift; break;
		        case Quadrant.Right: position.x += shift; break;
		        case Quadrant.Up: position.y += shift; break;
		        case Quadrant.Left: position.x -= shift; break;
		        default: index = default; return false;
	        }

	        return Tilemap.IndexesReference.Value.TryGetValue(position, out index);
        }
		
		private ref NativeTile GetTile(int index) => ref TilesBlob.Value.Tiles[index];
    }
}
