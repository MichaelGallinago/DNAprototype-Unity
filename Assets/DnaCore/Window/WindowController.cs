using System.Runtime.CompilerServices;

namespace DnaCore.Window
{
    public static class WindowController
    {
        public static WindowControllerInstance Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)] get => WindowControllerInstance.Instance;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Initialize(string name) => WindowControllerInstance.Initialize(name);
    }
}
