using System;
using Tiles.SolidTypes;
using UnityEngine;

namespace Tiles.Storage.Editor
{
    [Serializable]
    public struct TileKey
    {
        public Sprite Sprite;
        public SolidType SolidType;

        public TileKey(Sprite sprite, SolidType solidType)
        {
            Sprite = sprite;
            SolidType = solidType;
        }
    }
}
