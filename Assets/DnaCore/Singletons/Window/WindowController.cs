using System.Runtime.CompilerServices;
using DnaCore.Settings;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace DnaCore.Singletons.Window
{
    public static class WindowController
    {
        public static WindowControllerInstance Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)] get => WindowControllerInstance.Instance;
        }
        
        public static int Scale                                                                                                   
        {                                                                                                                  
            [MethodImpl(MethodImplOptions.AggressiveInlining)] get => Instance.Scale;                                                                           
            [MethodImpl(MethodImplOptions.AggressiveInlining)] set => Instance.Scale = value;                 
        }
                                                                                                                           
        public static AspectRatio Ratio
        {                                                                                                                  
            [MethodImpl(MethodImplOptions.AggressiveInlining)] get => Instance.Ratio;                                            
            [MethodImpl(MethodImplOptions.AggressiveInlining)] set => Instance.Ratio = value;                                                                                              
        }                                                                                                                  
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddCamera(Camera newCamera, PixelPerfectCamera pixelPerfectCamera) => 
            Instance.AddCamera(newCamera, pixelPerfectCamera);
    }
}
