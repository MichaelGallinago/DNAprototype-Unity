using System.IO;
using UnityEngine;

namespace PhysicsEcs2D.Tiles
{
    public static class TileConstants
    {
        public const int Size = 16;
        public const int DivSize = 4;
        public const int HalfSize = Size / 2;
        public const int ModSize = Size - 1;
        public const int PixelNumber = Size * Size;
        public static readonly Vector2Int CellSize = new(Size, Size);
        
        public static string BlobPath => Path.Combine(Application.streamingAssetsPath, "Blobs", "TileStorage.blob");
    }
}
