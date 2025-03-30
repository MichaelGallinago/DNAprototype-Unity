using System.Runtime.CompilerServices;
using Unity.Entities;

namespace DnaCore.Utilities.Ecs
{
    public static class BakerExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BakerQuery BakeQuery(this IBaker baker, TransformUsageFlags flags) => new(baker, flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BakerQuery BakeQuery(this IBaker baker, Entity entity) => new(baker, entity);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BakerQuery BakeQueryAdditional(this IBaker baker, TransformUsageFlags flags) => 
            BakerQuery.CreateAdditional(baker, flags);
    }
}
