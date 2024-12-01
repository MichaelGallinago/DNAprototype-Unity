
using UnityEngine;

namespace Tiles.Storage
{
    [System.Serializable]
    public struct SizeData
    {
        public byte[] Array;
        public Vector4 Angle;

        public SizeData(byte[] array, Vector4 angle)
        {
            Array = array;
            Angle = angle;
        }
    }
}
