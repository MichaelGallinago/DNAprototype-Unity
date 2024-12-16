using System.Runtime.CompilerServices;
using Tiles.Models;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Tiles.TileConstants;

namespace Tiles.Collision
{
	public readonly struct TileCollider
	{
		/*
		private const int MaxDistance = Size * 2;
		
	    public Vector2Int Position { get; set; }
	    public Quadrant Quadrant { get; set; }
	    
	    private readonly Tilemap _tilemap;
	    
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    public TileCollider(Tilemap tilemap, Quadrant quadrant, Vector2Int position)
	    {
		    _tilemap = tilemap;
		    Quadrant = quadrant;
		    Position = position;
	    }
	    
	    public (int distance, float angle) FindTile(int x, int y)
	    {
	        return GetTile(Position + new Vector2Int(x, y));
	    }
	    
	    public int FindDistance(int x, int y)
	    {
		    return GetDistance(Position + new Vector2Int(x, y));
	    }
	    
	    public (int distance, float angle) FindClosestTile(int x1, int y1, int x2, int y2)
	    {
		    (int distance, float) tile1 = GetTile(Position + new Vector2Int(x1, y1));
		    (int distance, float) tile2 = GetTile(Position + new Vector2Int(x2, y2));
		    
		    return tile1.distance <= tile2.distance ? tile1 : tile2;
	    }
	    
	    public int FindClosestDistance(int x1, int y1, int x2, int y2)
	    {
		    int distance1 = GetDistance(Position + new Vector2Int(x1, y1));
		    int distance2 = GetDistance(Position + new Vector2Int(x2, y2));
		    
		    return distance1 <= distance2 ? distance1 : distance2;
	    }
	    
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int GetDistance(Vector2Int targetPosition)
		{
			return !FindTileData(targetPosition, out int distance, out GeneratedTile _) ? MaxDistance : distance;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private (int, float) GetTile(Vector2Int targetPosition)
		{
			if (!FindTileData(targetPosition, out int distance, out GeneratedTile tile))
			{
				return (MaxDistance, float.NaN);
			}
			
			float angle = tile.GetAngle(Quadrant);
			return (distance, angle);
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool FindTileData(Vector2Int targetPosition, out int distance, out GeneratedTile tile)
		{
			var inTilePosition = new Vector2Int(targetPosition.x & ModSize, targetPosition.y & ModSize);
			targetPosition = new Vector2Int(inTilePosition.x >> 4, inTilePosition.y >> 4);
			
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool CheckFurtherTile(
			Vector2Int targetPosition, Vector2Int inTilePosition, out int distance, out GeneratedTile tile)
		{
			tile = Search(targetPosition, 1);
			distance = tile.GetSize(Quadrant, inTilePosition.x, inTilePosition.y);
					
			if (distance == 0)
			{
				tile = null;
				return false;
			}
					
			distance = CalculateInTilePosition(inTilePosition) - distance + Size;
			return true;
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool CheckCloserTile(
			Vector2Int targetPosition, Vector2Int inTilePosition, out int distance, ref GeneratedTile tile)
		{
			GeneratedTile closerTile = Search(targetPosition, -1);
			distance = closerTile.GetSize(Quadrant, inTilePosition.x, inTilePosition.y);
					
			if (distance > 0)
			{
				tile = closerTile;
			}
					
			distance = CalculateInTilePosition(inTilePosition) - distance - Size;
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int CalculateInTilePosition(Vector2Int inTilePosition) => Quadrant switch
		{
			Quadrant.Down => inTilePosition.y & ModSize,
			Quadrant.Right => ModSize - (inTilePosition.x & ModSize),
			Quadrant.Up => ModSize - (inTilePosition.y & ModSize),
			Quadrant.Left => inTilePosition.x & ModSize,
			_ => 0
		};
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private GeneratedTile Search(Vector2Int position, sbyte shift)
		{
			switch (Quadrant)
			{
				case Quadrant.Down: position.y -= shift; break;
				case Quadrant.Right: position.x += shift; break;
				case Quadrant.Up: position.y += shift; break;
				case Quadrant.Left: position.x -= shift; break;
				default: return null;
			}
			
			return _tilemap.GetTile<GeneratedTile>(new Vector3Int(position.x, position.y));
		}
		*/
	}
}
