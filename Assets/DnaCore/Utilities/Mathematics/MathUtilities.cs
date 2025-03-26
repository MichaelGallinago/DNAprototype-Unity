using Unity.Burst;
using Unity.Mathematics;

namespace DnaCore.Utilities.Mathematics
{
    [BurstCompile]
    public static class MathUtilities
    {
        public static float GetSector(float startRadians, float endRadians)
        {
            float radians = (endRadians - startRadians) % math.PI2;
            if (radians < 0) radians += math.PI2;
            return math.min(radians, math.PI2 - radians);
        }

        public static Quadrant GetQuadrant(float radians)
        {
            radians = radians % Circle.Full + (radians < 0f ? Circle.Full + Circle.OneEighth : Circle.OneEighth);
            return (Quadrant)((int)radians / 90 & 3);
        }

        public static float ProjectOnPlane(float2 vector, float radians) =>
            math.csum(vector * new float2(math.cos(radians), math.sin(radians)));
        
        public static float FloatToDb(float value) => 
            math.log10(math.clamp(value, 0.0001f, 1f)) * 20f;
        
        public static int FindGreatestCommonDivisor(int x, int y)
        {
            if (x == 0) return y;
            if (y == 0) return x;

            int2 vector = math.abs(new int2(x, y));
   
            var shift = 0;
            while (((vector.x | vector.y) & 1) == 0)
            {
                vector >>= 1;
                shift++;
            }
    
            while ((vector.x & 1) == 0)
            {
                vector.x >>= 1;
            }
    
            return OffsetVector(vector) << shift;
        }

        private static int OffsetVector(int2 vector)
        {
            do
            {
                while ((vector.y & 1) == 0)
                {
                    vector.y >>= 1;
                }

                if (vector.x > vector.y)
                {
                    (vector.x, vector.y) = (vector.y, vector.x);
                }

                vector.y -= vector.x;
            } while (vector.y != 0);

            return vector.x;
        }
    }
}
