using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;

namespace Utilities
{
    [BurstCompile]
    public static class SystemStateUtilities
    {
        [BurstCompile]
        public static JobHandle GetJobHandle<T>(this ref SystemState state) where T : unmanaged, ISystem
        {
            SystemHandle physicsSystemHandle = state.World.GetExistingSystem<T>();
            return state.World.Unmanaged.ResolveSystemStateRef(physicsSystemHandle).Dependency;
        }
    }
}
