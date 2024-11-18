using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Utilities
{
    public static class CursorUtilities
    {
        public static void Hide()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        public static void Show()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ShowInEditor(bool show) => ShowCursor(show);
        
#if UNITY_EDITOR_WIN
        [DllImport("user32.dll")]
        private static extern int ShowCursor(bool show);
#else
        private static void ShowCursor(bool show) => Debug.Log("GO FUCK YOURSELF LINUX & MACOS USERS");
#endif
    }
}