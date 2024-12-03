using Tiles.SolidTypes;
using UnityEngine;

namespace Tiles.Storage
{
    public readonly struct TileKey
    {
        public readonly Sprite Sprite;
        public readonly SolidType SolidType;

        public TileKey(Sprite sprite, SolidType solidType)
        {
            Sprite = sprite;
            SolidType = solidType;
        }
    }
}
