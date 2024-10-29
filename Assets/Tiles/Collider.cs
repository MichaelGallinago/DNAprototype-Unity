using Tiles.Generator;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utilities;

using static Tiles.TileUtilities;

namespace Tiles
{
	public class Collider
	{
		private const int MaxDistance = TileSize * 2;
		
	    public Vector2Int Position { get; set; }

	    public Layers LayerType
	    {
		    set
		    {
			    _tileMap = value switch
			    {
				    Layers.Main => SceneModule.Scene.Instance.CollisionTileMapMain,
				    Layers.Secondary => SceneModule.Scene.Instance.CollisionTileMapSecondary,
				    _ => null
			    };
		    }
	    }
	    
	    private Quadrant _quadrant;
	    private Tilemap _tileMap;
	    private sbyte _shift;
	    private byte _size;
	    
	    public void SetData(int x, int y, Layers type)
	    {
	        Position = new Vector2Int(x, y);
	        LayerType = type;
	    }
	    
	    public void SetData(Vector2Int position, Layers type)
	    {
		    Position = position;
		    LayerType = type;
	    }

	    public (int, float) FindTile(int x, int y, Quadrant quadrant)
	    {
		    _quadrant = quadrant;
	        return GetTile(Position + new Vector2Int(x, y));
	    }
	    
	    public int FindDistance(int x, int y, Quadrant quadrant)
	    {
		    _quadrant = quadrant;
		    return GetDistance(Position + new Vector2Int(x, y));
	    }

	    public (int, float) FindClosestTile(int x1, int y1, int x2, int y2, Quadrant quadrant)
	    {
		    _quadrant = quadrant;
		    
		    (int distance, float) tile1 = GetTile(Position + new Vector2Int(x1, y1));
		    (int distance, float) tile2 = GetTile(Position + new Vector2Int(x2, y2));
		    
		    return tile1.distance <= tile2.distance ? tile1 : tile2;
	    }
	    
	    public int FindClosestDistance(int x1, int y1, int x2, int y2, Quadrant quadrant)
	    {
		    _quadrant = quadrant;
		    
		    int distance1 = GetDistance(Position + new Vector2Int(x1, y1));
		    int distance2 = GetDistance(Position + new Vector2Int(x2, y2));
		    
		    return distance1 <= distance2 ? distance1 : distance2;
	    }
	    
		private int GetDistance(Vector2Int targetPosition)
		{
			return !FindTileData(targetPosition, out int distance, out GeneratedTile _) ? MaxDistance : distance;
		}

		private (int, float) GetTile(Vector2Int targetPosition)
		{
			if (!FindTileData(targetPosition, out int distance, out GeneratedTile tile))
			{
				return (MaxDistance, float.NaN);
			}
			
			float angle = tile.GetAngle(_quadrant);
			return (distance, angle);
		}
		
		private bool FindTileData(Vector2Int targetPosition, out int distance, out GeneratedTile tile)
		{
			tile = Search(0);
			distance = tile.GetSize(_quadrant, _targetPosition.x, _targetPosition.y);
			
			switch (distance)
			{
				// Further tile check
				case 0:
				{
					tile = Search(TileSize);
					distance = tile.GetSize(_quadrant, _targetPosition.x, _targetPosition.y);
					if (distance == 0)
					{
						tile = null;
						return false;
					}
					distance = CalculateInTilePosition() - distance + TileSize;
					return true;
				}
				// Closer tile check
				case TileSize: 
				{
					GeneratedTile closerTile = Search(-TileSize);
					distance = closerTile.GetSize(_quadrant, _targetPosition.x, _targetPosition.y);
					
					if (distance > 0)
					{
						tile = closerTile;
					}
					distance = CalculateInTilePosition() - distance - TileSize;
					return true;
				}
				default:
					distance = CalculateInTilePosition() - distance;
					return true;
			}
		}

		private int CalculateInTilePosition() => _quadrant switch
		{
			Quadrant.Down => ModTileSize - (_targetPosition.y & ModTileSize),
			Quadrant.Right => ModTileSize - (_targetPosition.x & ModTileSize),
			Quadrant.Up => _targetPosition.y & ModTileSize,
			Quadrant.Left => _targetPosition.x & ModTileSize,
			_ => 0
		};
		
		private GeneratedTile Search(sbyte shift)
		{
			Vector2Int position = _targetPosition;
			
			switch (_quadrant)
			{
				case Quadrant.Down: position.y += shift; break;
				case Quadrant.Right: position.x += shift; break;
				case Quadrant.Up: position.y -= shift; break;
				case Quadrant.Left: position.x -= shift; break;
				default: return null;
			}
			
			return _tileMap.GetTile<GeneratedTile>(new Vector3Int(position.x >> 4, position.y >> 4));
		}
	}
}
