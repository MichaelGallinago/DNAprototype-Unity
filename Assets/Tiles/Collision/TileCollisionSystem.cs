using Character;
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
            else
            {
                Debug.Log(_tilemap.IndexesReference.Value.Count);
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
        public BlobAssetReference<TilesBlob> TilesBlob;
        public NativeTilemap Tilemap;
        
        private void Execute(in LocalTransform transform, ref TileSensor sensor)
        {
            sensor.Distance = 4;

            //transform.Position.xy += velocity.Vector.GetValueDelta(Speed);
            //velocity.Vector.ResetInstanceValue();
        }
        
		private bool FindTileData(int2 targetPosition, out int distance, out GeneratedTile tile)
		{
			var inTilePosition = new int2(targetPosition.x & ModSize, targetPosition.y & ModSize);
			targetPosition = new int2(inTilePosition.x >> 4, inTilePosition.y >> 4);
			
			tile = Search(targetPosition, 0);
			
			distance = tile.GetSize(Quadrant, inTilePosition.x, inTilePosition.y);
			
			switch (distance)
			{
				case 0: return CheckFurtherTile(targetPosition, inTilePosition, out distance, out tile);
				case Size: return CheckCloserTile(targetPosition, inTilePosition, out distance, ref tile);
				default:
					distance = CalculateInTilePosition(inTilePosition) - distance;
					return true;
			}
		}
		
		private bool CheckFurtherTile(
			int2 targetPosition, int2 inTilePosition, out int distance, ref NativeTile tile)
		{
			if (TrySearch(targetPosition, 1, out int index))
			{
				ref NativeTile closerTile = ref GetTile(index);
			}
			
			distance = tile.GetSize(Quadrant, inTilePosition.x, inTilePosition.y);
					
			if (distance == 0)
			{
				tile = null;
				return false;
			}
					
			distance = CalculateInTilePosition(inTilePosition) - distance + Size;
			return true;
		}
		
		private bool CheckCloserTile(int2 targetPosition, int2 inTilePosition, out int distance, ref NativeTile tile)
		{
			if (TrySearch(targetPosition, -1, out int index))
			{
				ref NativeTile closerTile = ref GetTile(index);
				distance = closerTile.GetSize(Quadrant, inTilePosition.x, inTilePosition.y);
				
				if (distance > 0)
				{
					tile = closerTile;
				}
			}
			else
			{
				distance = 0;
			}
			
			distance = CalculateInTilePosition(inTilePosition) - distance - Size;
			return true;
		}
		
		private int CalculateInTilePosition(int2 inTilePosition) => Quadrant switch
		{
			Quadrant.Down => inTilePosition.y & ModSize,
			Quadrant.Right => ModSize - (inTilePosition.x & ModSize),
			Quadrant.Up => ModSize - (inTilePosition.y & ModSize),
			Quadrant.Left => inTilePosition.x & ModSize,
			_ => 0
		};
		
		private bool TrySearch(int2 position, sbyte shift, out int index)
		{
			switch (Quadrant)
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
