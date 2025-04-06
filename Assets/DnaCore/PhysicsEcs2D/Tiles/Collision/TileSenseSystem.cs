using DnaCore.PhysicsEcs2D.Tiles.Generators.TilemapGenerator;
using DnaCore.Utilities.Mathematics;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using static DnaCore.PhysicsEcs2D.Tiles.TileConstants;

namespace DnaCore.PhysicsEcs2D.Tiles.Collision
{
    [BurstCompile]
    [UpdateAfter(typeof(SensorAdjustSystem))]
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
            state.RequireForUpdate<TileSensor>();
        }
        
        public void OnStartRunning(ref SystemState state)
        {
            if (SystemAPI.TryGetSingleton(out _tilemap)) return;
            Debug.LogError("No " + nameof(NativeTilemap) + " found");
        }

        public void OnUpdate(ref SystemState state) => state.Dependency = new TileSenseJob
        {
	        TilesBlob = _tilesBlob,
	        Tilemap = _tilemap
        }.ScheduleParallel(state.Dependency);

        public readonly void OnStopRunning(ref SystemState state) {}
        
        public void OnDestroy(ref SystemState state)
        {
            if (!_tilesBlob.IsCreated) return;
	        _tilesBlob.Dispose();
        }
    }

    [BurstCompile]
    public partial struct TileSenseJob : IJobEntity
    {
	    private const int MaxDistance = Size * 2;
	    
	    [ReadOnly] public BlobAssetReference<TilesBlob> TilesBlob;
	    [ReadOnly] public NativeTilemap Tilemap;
	    
        private void Execute(ref TileSensor sensor)
        {
	        int2 inTilePosition = sensor.Position & ModSize;
	        int2 tilePosition = sensor.Position >> DivSize;

	        if (!TrySearch(tilePosition, 0, sensor.Quadrant, out int index))
	        {
		        FindFurtherTile(tilePosition, inTilePosition, ref sensor);
		        return;
	        }

	        ref NativeTile tile = ref GetTile(index);
	        byte size = tile.GetSize(sensor.Quadrant, inTilePosition);

	        switch (size)
	        {
		        case 0:
			        FindFurtherTile(tilePosition, inTilePosition, ref sensor);
			        break;
		        case Size:
			        sensor.Radians = tile.GetAngle(sensor.Quadrant);
			        FindCloserTile(tilePosition, inTilePosition, ref sensor);
			        break;
		        default:
			        sensor.Distance = CalculateInTilePosition(inTilePosition, sensor.Quadrant) - size;
			        sensor.Radians = tile.GetAngle(sensor.Quadrant);
			        break;
	        }
        }

        private void FindFurtherTile(int2 tilePosition, int2 inTilePosition, ref TileSensor sensor)
        {
	        if (TrySearch(tilePosition, 1, sensor.Quadrant, out int index))
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

        private void FindCloserTile(int2 tilePosition, int2 inTilePosition, ref TileSensor sensor)
        {
	        byte size;
	        if (TrySearch(tilePosition, -1, sensor.Quadrant, out int index))
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
        
        private bool TrySearch(int2 tilePosition, sbyte shift, Quadrant quadrant, out int index)
        {
	        switch (quadrant)
	        {
		        case Quadrant.Down: tilePosition.y -= shift; break;
		        case Quadrant.Right: tilePosition.x += shift; break;
		        case Quadrant.Up: tilePosition.y += shift; break;
		        case Quadrant.Left: tilePosition.x -= shift; break;
		        default: index = 0; return false;
	        }

	        return Tilemap.IndexesReference.Value.TryGetValue(tilePosition, out index);
        }
		
		private ref NativeTile GetTile(int index) => ref TilesBlob.Value.Tiles[index];
    }
}
