using System.Runtime.CompilerServices;
using Unity.Entities;

namespace DnaCore.Utilities
{
    public static class BlobUtilities
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Fill<T>(this ref BlobBuilder builder, ref BlobArray<T> pointer, T[] source) 
            where T : unmanaged
        {
            BlobBuilderArray<T> sizes = builder.Allocate(ref pointer, source.Length);

            for (var i = 0; i < source.Length; i++)
            {
                sizes[i] = source[i];
            }
        }
    }
}
