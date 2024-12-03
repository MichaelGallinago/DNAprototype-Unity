using System;
using System.Runtime.CompilerServices;
using Tiles.SolidTypes;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tiles.Generator
{
    [Serializable]
    public class GeneratedTile : TileBase
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public SolidType SolidType { get; private set; }
        
        [SerializeReference] private byte[] _heightsDown;
        [SerializeReference] private byte[] _widthsRight;
        [SerializeReference] private byte[] _heightsUp;
        [SerializeReference] private byte[] _widthsLeft;
        [SerializeField] private Vector4 _angles;

        public static GeneratedTile Create(
            SolidType solidType, Sprite sprite, Vector4 angles,
            byte[] heightsDown, byte[] widthsRight, byte[] heightsUp, byte[] widthsLeft)
        {
            var tile = CreateInstance<GeneratedTile>();
            tile.Sprite = sprite;
            tile.SolidType = solidType;
            tile._heightsDown = heightsDown;
            tile._widthsRight = widthsRight;
            tile._heightsUp = heightsUp;
            tile._widthsLeft = widthsLeft;
            tile._angles = angles;
            return tile;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetSize(Quadrant quadrant, int x, int y) => quadrant switch
        {
            Quadrant.Down => _heightsDown[x],
            Quadrant.Right => _widthsRight[y],
            Quadrant.Up => _heightsUp[x],
            Quadrant.Left => _widthsLeft[y],
            _ => default
        };
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float GetAngle(Quadrant quadrant) => quadrant switch
        {
            Quadrant.Down => _angles.x,
            Quadrant.Right => _angles.y,
            Quadrant.Up => _angles.z,
            Quadrant.Left => _angles.w,
            _ => float.NaN
        };
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (byte, float) GetCollisionData(Quadrant quadrant, int x, int y)
        {
            return (GetSize(quadrant, x, y), GetAngle(quadrant));
        }
        
        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.sprite = Sprite;
            tileData.color = SolidType.ToColor();
            tileData.transform = Matrix4x4.identity;
            tileData.gameObject = null;
            tileData.flags = TileFlags.LockAll;
            tileData.colliderType = Tile.ColliderType.None;
        }
    }
}
