
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DnaCore.Utilities.Mathematics
{
    public static class MatrixUtilities
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 Get(int x, int y, int scaleX, int scaleY) => new()
        {
            m00 = scaleX,
            m03 = x,
            m11 = scaleY,
            m13 = y,
            m22 = 1f,
            m33 = 1f
        };
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 Get(int x, int y) => new()
        {
            m00 = 1f,
            m03 = x,
            m11 = 1f,
            m13 = y,
            m22 = 1f,
            m33 = 1f
        };
    }
}
