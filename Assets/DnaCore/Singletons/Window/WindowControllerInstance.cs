using DnaCore.Settings;
using DnaCore.Singletons.Window.RenderTexture;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using AspectRatio = DnaCore.Settings.AspectRatio;

namespace DnaCore.Singletons.Window
{
    public class WindowControllerInstance : MonoSingleton<WindowControllerInstance>
    {                          
        [SerializeField] private RenderTextureMatcher _renderTextureMatcher;
        
        private Camera _camera;
        private PixelPerfectCamera _pixelPerfectCamera;

        public int Scale
        {
            get => AppSettings.Options.Scale;
            set
            {
                AppSettings.Options.Scale = value;
                if (value == 0)
                {
                    DisplayInfo displayInfo = Screen.mainWindowDisplayInfo;
                    Screen.SetResolution(displayInfo.width, displayInfo.height, FullScreenMode.FullScreenWindow);
                    return;
                }
                
                Vector2Int resolution = Ratio.GetScaledResolution(value);
                Screen.SetResolution(resolution.x, resolution.y, FullScreenMode.Windowed);
            }
        }

        public AspectRatio Ratio
        {
            get => AppSettings.Options.AspectRatio;
            set
            {
                AppSettings.Options.AspectRatio = value;
                Vector2Int minResolution = value.MinResolution;
                SetupPixelPerfectCamera(minResolution);
                
                DisplayInfo displayInfo = Screen.mainWindowDisplayInfo;
                Vector2Int resolution;
                int scale = Scale + 1;
                do
                {
                    resolution = minResolution * scale;
                    scale--;
                } while ((resolution.x > displayInfo.width || resolution.y > displayInfo.height) && scale != 0);

                _renderTextureMatcher.MatchRenderTexture(minResolution);
                Scale = scale;
            }
        }

        protected override void Initialize()
        {
            Ratio = AppSettings.Options.AspectRatio;
            Scale = AppSettings.Options.Scale;
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

            SetupPixelPerfectCamera(AppSettings.Options.AspectRatio.MinResolution);
        }

        private void SetupPixelPerfectCamera(Vector2Int resolution)
        {
            if (!_pixelPerfectCamera) return;
            _pixelPerfectCamera.refResolutionX = resolution.x;
            _pixelPerfectCamera.refResolutionY = resolution.y;
        }
    }
}
