using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Text;
using UnityEngine;

namespace DnaCore.Settings
{
    [Serializable]
    public struct AspectRatio : IEquatable<AspectRatio>
    {
        public static readonly AspectRatio Reference = new(16, 9);
        public static readonly Vector2Int ReferenceResolution = new(640, 360);
        
        public static readonly AspectRatio[] BuiltIn =
        {
            Reference, new(8, 5), new(7, 3), new(32, 9), new(4, 3)
        };

        public static readonly Dictionary<AspectRatio, string> AspectNameOverrides = new()
        {
            [new AspectRatio(8, 5)] = "16:10",
            [new AspectRatio(7, 3)] = "21:9",
            [new AspectRatio(39, 18)] = "19.5:9",
            [new AspectRatio(2, 1)] = "18:9"
        };
        
        public int X;
        public int Y;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public AspectRatio(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public AspectRatio(int width, int height, int greatestCommonDivisor)
        {
            X = width / greatestCommonDivisor;
            Y = height / greatestCommonDivisor;
        }

        public float Ratio
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)] get => (float)X / Y;
        }
        
        public Vector2Int MinResolution
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)] get => Ratio >= Reference.Ratio 
                ? new Vector2Int((int)((float)ReferenceResolution.y / Reference.Y * X), ReferenceResolution.y) 
                : new Vector2Int(ReferenceResolution.x, (int)((float)ReferenceResolution.x / Reference.X * Y));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2Int GetScaledResolution(int scale) => MinResolution * scale;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => ZString.Concat(X, ':', Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.Combine(X, Y);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is AspectRatio ratio && Equals(ratio);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(AspectRatio ratio) => ratio.X == X && ratio.Y == Y;
    }
}
