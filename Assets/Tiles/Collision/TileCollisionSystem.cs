using Character.TileSensor;
using Tiles.Generators;
using Tiles.Models;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static Tiles.TileConstants;

namespace Tiles.Collision
{
    [BurstCompile]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial struct TileCollisionSystem : ISystem, ISystemStartStop
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
        
        public void OnUpdate(ref SystemState state)
        {
            new TileCollisionJob { TilesBlob = _tilesBlob, Tilemap = _tilemap }.ScheduleParallel();
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
    
    [BurstCompile]
    public partial struct TileCollisionJob : IJobEntity
    {
	    private const int MaxDistance = Size * 2;
	    
        public BlobAssetReference<TilesBlob> TilesBlob;
        public NativeTilemap Tilemap;

        private void Execute(ref LocalToWorld transform, ref TileSensor sensor)
        {
	        FindTileData((int2)math.floor(transform.Position.xy), ref sensor);
	        //FindTileData((int2)math.floor(new float2(0f, 160f)), ref sensor);
	        Debug.Log($"{transform.Position.x}: {sensor.Distance}");
        }

        private void FindTileData(int2 targetPosition, ref TileSensor sensor)
        {
	        int2 inTilePosition = targetPosition % Size;
	        targetPosition /= Size;

	        if (!TrySearch(targetPosition, 0, sensor.Quadrant, out int index))
	        {
		        //Debug.Log($"{Tilemap.IndexesReference.Value[new int2(0, 10)]}");
		        FindFurtherTile(targetPosition, inTilePosition, ref sensor);
		        return;
	        }

	        ref NativeTile tile = ref GetTile(index);
	        byte distance = tile.GetSize(sensor.Quadrant, inTilePosition);

	        switch (distance)
	        {
		        case 0:
			        FindFurtherTile(targetPosition, inTilePosition, ref sensor);
			        break;
		        case Size:
			        sensor.Angle = tile.GetAngle(sensor.Quadrant);
			        FindCloserTile(targetPosition, inTilePosition, ref sensor);
			        break;
		        default:
			        sensor.Distance = CalculateInTilePosition(inTilePosition, sensor.Quadrant) - distance;
			        sensor.Angle = tile.GetAngle(sensor.Quadrant);
			        break;
	        }
        }

        private void FindFurtherTile(int2 targetPosition, int2 inTilePosition, ref TileSensor sensor)
        {
	        if (TrySearch(targetPosition, 1, sensor.Quadrant, out int index))
	        {
		        ref NativeTile tile = ref GetTile(index);
		        byte distance = tile.GetSize(sensor.Quadrant, inTilePosition);

		        if (distance > 0)
		        {
			        sensor.Distance = CalculateInTilePosition(inTilePosition, sensor.Quadrant) - distance + Size;
			        sensor.Angle = tile.GetAngle(sensor.Quadrant);
			        return;
		        }
	        }

	        sensor.Distance = MaxDistance;
	        sensor.Angle = float.NaN;
        }

        private void FindCloserTile(int2 targetPosition, int2 inTilePosition, ref TileSensor sensor)
        {
	        byte distance;
	        if (TrySearch(targetPosition, -1, sensor.Quadrant, out int index))
	        {
		        ref NativeTile tile = ref GetTile(index);
		        distance = tile.GetSize(sensor.Quadrant, inTilePosition);

		        if (distance > 0)
		        {
			        sensor.Angle = tile.GetAngle(sensor.Quadrant);
		        }
	        }
	        else
	        {
		        distance = 0;
	        }

	        sensor.Distance = CalculateInTilePosition(inTilePosition, sensor.Quadrant) - distance - Size;
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
