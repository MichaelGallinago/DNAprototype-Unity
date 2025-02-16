using Unity.Burst;
using Unity.Mathematics;

namespace DnaCore.Utilities
{
    [BurstCompile]
    public static class MathUtilities
    {
        public static float ProjectOnPlane(float2 vector, float angle) => 
            math.csum(vector * math.abs(new float2(math.cos(angle), math.sin(angle))));
    }
}
