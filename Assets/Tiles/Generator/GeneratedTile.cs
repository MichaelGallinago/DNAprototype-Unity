using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Tiles.TileUtilities;

namespace Tiles.Generator
{
    [Serializable]
    public class GeneratedTile : TileBase
    {
        public enum SolidType : byte { Full, Top, NoTop }
        
        [field: SerializeField] public Sprite Sprite { get; set; }
        
        [SerializeField] private Matrix4x4 _transform = Matrix4x4.identity;
        [SerializeField] private Color _color = Color.white;
        
        [SerializeField] private byte[] _heightsDown;
        [SerializeField] private byte[] _widthsRight;
        [SerializeField] private byte[] _heightsUp;
        [SerializeField] private byte[] _widthsLeft;
        [SerializeField] private Vector4 _angles;
        
        public Matrix4x4 Transform { get => _transform; set => _transform = value; }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetSize(Quadrant quadrant, int x, int y)
        {
            return quadrant switch
            {
                Quadrant.Down => _heightsDown[x],
                Quadrant.Right => _widthsRight[y],
                Quadrant.Up => _heightsUp[x],
                Quadrant.Left => _widthsLeft[y],
                _ => 0
            };
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float GetAngle(Quadrant quadrant)
        {
            return quadrant switch
            {
                Quadrant.Down => _angles.x,
                Quadrant.Right => _angles.y,
                Quadrant.Up => _angles.z,
                Quadrant.Left => _angles.w,
                _ => float.NaN
            };
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (byte, float) GetCollisionData(Quadrant quadrant, int x, int y)
        {
            return (GetSize(quadrant, x, y), GetAngle(quadrant));
        }

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.sprite = Sprite;
            tileData.color = _color;
            tileData.transform = _transform;
            tileData.gameObject = null;
            tileData.flags = TileFlags.LockAll;
            tileData.colliderType = Tile.ColliderType.None;
        }
    }
}
