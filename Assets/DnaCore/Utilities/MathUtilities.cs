using System;
using Unity.Burst;
using Unity.Mathematics;

namespace DnaCore.Utilities
{
    [BurstCompile]
    public static class MathUtilities
    {
        public static float ProjectOnPlane(float2 vector, float angle) => 
            math.csum(vector * math.abs(new float2(math.cos(angle), math.sin(angle))));
        
        public static float FloatToDb(float value) => 
            math.log10(math.clamp(value, 0.0001f, 1f)) * 20f;
        
        public static int FindGreatestCommonDivisor(int x, int y)
        {
            if (x == 0) return y;
            if (y == 0) return x;

            x = math.abs(x);
            y = math.abs(y);
   
            var shift = 0;
            while (((x | y) & 1) == 0)
            {
                x >>= 1;
                y >>= 1;
                shift++;
            }
    
            while ((x & 1) == 0) 
            {
                x >>= 1;
            }
    
            do
            {
                while ((y & 1) == 0) 
                {
                    y >>= 1;
                }

                if (x > y)
                {
                    (x, y) = (y, x);
                }
    
                y -= x;
            } while (y != 0);
    
            return x << shift;
        }
    }
}
