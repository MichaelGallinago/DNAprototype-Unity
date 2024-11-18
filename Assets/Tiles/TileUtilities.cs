using Tiles.Generator;
using UnityEngine;

namespace Tiles
{
    public static class TileUtilities
    {
        public const int Size = 16;
        public const int ModSize = Size - 1;

        public static Color GetColor(this GeneratedTile.SolidType solidType) => solidType switch
        {
            GeneratedTile.SolidType.Full => Color.black,
            GeneratedTile.SolidType.Top => Color.white,
            GeneratedTile.SolidType.NoTop => Color.yellow,
            _ => Color.black
        };
    }
}