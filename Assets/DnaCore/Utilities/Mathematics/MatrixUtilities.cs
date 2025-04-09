
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace DnaCore.Utilities.Mathematics
{
    public static class MatrixUtilities
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 Get(int2 position, int scaleX, int scaleY) => new()
        {
            m00 = scaleX,
            m03 = position.x,
            m11 = scaleY,
            m13 = position.y,
            m22 = 1f,
            m33 = 1f
        };
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 Get(int2 position) => new()
        {
            m00 = 1f,
            m03 = position.x,
            m11 = 1f,
            m13 = position.y,
            m22 = 1f,
            m33 = 1f
        };
    }
}
