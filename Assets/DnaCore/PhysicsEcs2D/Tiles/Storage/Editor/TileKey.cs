using System;
using DnaCore.PhysicsEcs2D.Tiles.SolidTypes;
using UnityEngine;

namespace DnaCore.PhysicsEcs2D.Tiles.Storage.Editor
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
