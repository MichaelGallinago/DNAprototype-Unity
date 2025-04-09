using Unity.Burst;
using UnityEngine;

namespace DnaCore.Utilities.Ecs
{
    [BurstCompile]
    public static class Bursted
    {
#if UNITY_EDITOR
        public static void Log<T>(T x) => Debug.Log($"{x}");
        public static void Log<T1, T2>(T1 x, T2 y) => Debug.Log($"{x} : {y}");
        public static void Log<T1, T2, T3>(T1 x, T2 y, T3 z) => Debug.Log($"{x} : {y} : {z}");
        public static void Log<T1, T2, T3, T4>(T1 x, T2 y, T3 z, T4 w) => Debug.Log($"{x} : {y} : {z} : {w}");  
#endif
    }
}
