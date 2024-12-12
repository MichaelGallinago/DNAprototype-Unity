using System.Runtime.CompilerServices;
using Unity.Entities;

namespace Utilities
{
    public static class BlobUtilities
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Fill<T>(this ref BlobArray<T> pointer, BlobBuilder builder, T[] source) where T : unmanaged
        {
            BlobBuilderArray<T> sizes = builder.Allocate(ref pointer, source.Length);

            for (var i = 0; i < source.Length; i++)
            {
                sizes[i] = source[i];
            }
        }
        
        //public static string GetPath() => 
        //    Path.Combine(Application.streamingAssetsPath, SceneManager., "Sus");
    }
}