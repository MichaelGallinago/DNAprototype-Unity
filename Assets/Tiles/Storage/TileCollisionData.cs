using System;
using UnityEngine;

namespace Tiles.Storage
{
    [Serializable]
    public struct TileCollisionData
    {
        [SerializeReference] public byte[] HeightsDown;
        [SerializeReference] public byte[] WidthsRight;
        [SerializeReference] public byte[] HeightsUp;
        [SerializeReference] public byte[] WidthsLeft;
        [SerializeField] public Vector4 Angles;

        public TileCollisionData(
            byte[] heightsDown, byte[] widthsRight, byte[] heightsUp, byte[] widthsLeft, Vector4 angles)
        {
            HeightsDown = heightsDown;
            WidthsRight = widthsRight;
            HeightsUp = heightsUp;
            WidthsLeft = widthsLeft;
            Angles = angles;
        }
    }
}
