using DnaCore.Settings;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace DnaCore.Window
{
    public class WindowControllerInstance : MonoSingleton<WindowControllerInstance>
    {                                                          
        private Camera _camera;
        private PixelPerfectCamera _pixelPerfectCamera;

        public int Scale
        {
            get => AppSettings.Options.Scale;
            set
            {
                Vector2Int resolution = AppSettings.Options.AspectRatio.GetScaledResolution(value + 1);                     
                Screen.SetResolution(resolution.x, resolution.y, Screen.fullScreenMode); 
            }
        }

        public bool FullScreen
        {
            get => AppSettings.Options.FullScreen;
            set => Screen.fullScreen = AppSettings.Options.FullScreen = value;
        }

        public AspectRatio Ratio
        {
            get => AppSettings.Options.AspectRatio;
            set
            {
                Vector2Int minResolution = value.MinResolution;
                _pixelPerfectCamera.refResolutionX = minResolution.x;
                _pixelPerfectCamera.refResolutionY = minResolution.y;
                
                DisplayInfo currentInfo = Screen.mainWindowDisplayInfo;                                                
                Vector2Int resolution;                                                                                 
                int scale = AppSettings.Options.Scale + 1;                                                             
                do                                                                                                     
                {                                                                                                      
                    resolution = minResolution * scale;                                                                
                    scale--;                                                                                           
                } while (resolution.x > currentInfo.width || resolution.y > currentInfo.height || scale == 0);                                           
                                          
                Screen.SetResolution(resolution.x, resolution.y, Screen.fullScreenMode);
            }
        }
        
        /// <summary>
        /// Adds a new camera. Expand to have multiple cameras if necessary.
        /// </summary>
        /// <param name="newCamera"></param>
        /// <param name="pixelPerfectCamera"></param>
        public void AddCamera(Camera newCamera, PixelPerfectCamera pixelPerfectCamera)
        {
            _camera = newCamera;
            _pixelPerfectCamera = pixelPerfectCamera;
                                                                     
            Vector2Int minResolution = AppSettings.Options.AspectRatio.MinResolution;      
            _pixelPerfectCamera.refResolutionX = minResolution.x;
            _pixelPerfectCamera.refResolutionY = minResolution.y;
        }
    }
}
