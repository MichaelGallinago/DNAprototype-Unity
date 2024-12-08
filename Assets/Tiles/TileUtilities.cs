using UnityEngine;

namespace Tiles
{
    public static class TileUtilities
    {
        public const int Size = 16;
        public const int ModSize = Size - 1;
        public const int PixelNumber = Size * Size;
        public static readonly Vector2Int CellSize = new(Size, Size);
    }
}
