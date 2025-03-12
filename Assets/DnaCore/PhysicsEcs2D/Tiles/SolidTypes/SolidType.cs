using System;
using UnityEngine;

namespace DnaCore.PhysicsEcs2D.Tiles.SolidTypes
{
    public enum SolidType : byte { Full, Top, NoTop }
    
    public static class SolidTypeExtensions
    {
        public static readonly SolidType[] Values = (SolidType[])Enum.GetValues(typeof(SolidType));
        public static readonly int Number = Values.Length;
        
        public static Color ToColor(this SolidType solidType) => solidType switch
        {
            SolidType.Full => Color.black,
            SolidType.Top => Color.white,
            SolidType.NoTop => Color.yellow,
            _ => Color.magenta
        };
    }
}
