using System;
using System.Runtime.CompilerServices;
using UnityEngine;

using static System.Runtime.CompilerServices.MethodImplOptions;

namespace DnaCore.Utilities
{
    public static class VectorUtilities
    {
        [MethodImpl(AggressiveInlining)]
        public static float SquaredDistance(Vector2 a, Vector2 b)
        {
            float num1 = a.x - b.x;
            float num2 = a.y - b.y;
            return num1 * num1 + num2 * num2;
        }
        
        [MethodImpl(AggressiveInlining)]
        public static float Distance(Vector2 a, Vector2 b) => MathF.Sqrt(SquaredDistance(a, b));

        [MethodImpl(AggressiveInlining)]
        public static Vector2Int ToInt(this Vector2 vector) => new((int)vector.x, (int)vector.y);
        
        [MethodImpl(AggressiveInlining)]
        public static Vector2Int ToVector2Int(this Vector3 vector) => new((int)vector.x, (int)vector.y);
        
        [MethodImpl(AggressiveInlining)]
        public static Vector2Int ToVector2Int(this Vector3Int vector) => new(vector.x, vector.y);
        
        [MethodImpl(AggressiveInlining)]
        public static Vector3 ToVector3(this Vector2Int vector) => new(vector.x, vector.y);
        
        [MethodImpl(AggressiveInlining)]
        public static Vector2Int Abs(this Vector2Int vector) => new(Math.Abs(vector.x), Math.Abs(vector.y));
    }
}
