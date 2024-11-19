using Tiles.Generator;
using Tiles.SolidTypes;
using UnityEngine;

namespace Tiles
{
    public static class TileUtilities
    {
        public const int Size = 16;
        public const int ModSize = Size - 1;
        
        public static Color GetColor(this SolidType solidType) => solidType switch
        {
            SolidType.Full => Color.black,
            SolidType.Top => Color.white,
            SolidType.NoTop => Color.yellow,
            _ => Color.black
        };
    }
}