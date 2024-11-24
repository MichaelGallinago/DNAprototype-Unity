#if UNITY_EDITOR
using System;
using UnityEngine;

namespace Tiles.SolidTypes
{
    public enum SolidType : byte { Full, Top, NoTop }
    
    public static class SolidTypeExtensions
    {
        public static readonly int Number = Enum.GetValues(typeof(SolidType)).Length;
        
        public static Color GetColor(this SolidType solidType) => solidType switch
        {
            SolidType.Full => Color.black,
            SolidType.Top => Color.white,
            SolidType.NoTop => Color.yellow,
            _ => Color.black
        };
    }
}
#endif
