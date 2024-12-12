
using UnityEngine;

namespace Tiles.Storage
{
    [System.Serializable]
    public struct SizeData
    {
        public int Count;
        public byte[] Array;
        public Vector4 Angle;

        public SizeData(int count, byte[] array, Vector4 angle)
        {
            Count = count;
            Array = array;
            Angle = angle;
        }
    }
}
