using System;
using Tiles.SolidTypes;
using Tiles.Storage;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tiles.Generators
{
    [Serializable]
    public class GeneratedTile : TileBase
    {
        [field: SerializeField] public SolidType SolidType { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        
        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.sprite = Sprite;
            tileData.color = SolidType.ToColor();
            tileData.transform = Matrix4x4.identity;
            tileData.gameObject = null;
            tileData.flags = TileFlags.None;
            tileData.colliderType = Tile.ColliderType.None;
        }
        
#if UNITY_EDITOR
        [field: SerializeField] public int Index { get; private set; }
        [field: SerializeField] public int Count { get; set; }
        [field: SerializeField] public TileCollisionData CollisionData { get; private set; }

        public static GeneratedTile Create(
            SolidType solidType, Sprite sprite, int index, TileCollisionData collisionData)
        {
            var tile = CreateInstance<GeneratedTile>();
            tile.Sprite = sprite;
            tile.SolidType = solidType;
            tile.Index = index;
            tile.CollisionData = collisionData;
            tile.Count = 1;
            
            return tile;
        }
#else
        public static GeneratedTile Create(SolidType solidType, Sprite sprite)
        {
            var tile = CreateInstance<GeneratedTile>();
            tile.Sprite = sprite;
            tile.SolidType = solidType;
            
            return tile;
        }
#endif
    }
}
