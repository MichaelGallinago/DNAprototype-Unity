using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Extensions
{
    public static class VectorUtilities
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SquaredDistance(Vector2 a, Vector2 b)
        {
            float num1 = a.x - b.x;
            float num2 = a.y - b.y;
            return num1 * num1 + num2 * num2;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(Vector2 a, Vector2 b) => MathF.Sqrt(SquaredDistance(a, b));
    }
}
