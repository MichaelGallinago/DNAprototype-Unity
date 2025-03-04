using System.Runtime.CompilerServices;
using UnityEngine;

namespace DnaCore.Window
{
    public static class WindowController
    {
        public static WindowControllerInstance Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => WindowControllerInstance.Instance;
        }

        public static Resolution ReferenceResolution
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Instance.ReferenceResolution;
        }
    }
}
