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
            Reference, new(8, 5), new(4, 3), new(7, 3), new(32, 9)
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
                ? new Vector2Int(
                    (int)((float)ReferenceResolution.x * X * Reference.Y / Reference.X / Y), ReferenceResolution.y) 
                : new Vector2Int(
                    ReferenceResolution.x, (int)((float)ReferenceResolution.y * Y * Reference.X / Reference.Y / X));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetMaxScale(int maxWidth, int maxHeight)
        {
            Vector2Int minResolution = MinResolution;
            return Math.Min(maxWidth / minResolution.x, maxHeight / minResolution.y);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string[] GetResolutionNames(DisplayInfo displayInfo)
        {
            int count = GetMaxScale(displayInfo.width, displayInfo.height);
            var aspectRatios = new string[count];
            for (var i = 0; i < count; i++)
            {
                Vector2Int resolution = GetScaledResolution(i + 1);
                aspectRatios[i] = ZString.Concat(resolution.x, ":", resolution.y);
            }
            return aspectRatios;
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
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string[] GetRatiosNames(AspectRatio[] ratios)
        {
            var aspectRatios = new string[ratios.Length];
            for (var i = 0; i < ratios.Length; i++)
            {
                AspectRatio ratio = ratios[i];
                if (AspectNameOverrides.TryGetValue(ratio, out string aspectName))
                {
                    aspectRatios[i] = aspectName;
                    continue;
                }
                aspectRatios[i] = ratio.ToString();
            }
            
            return aspectRatios;
        }
    }
}
