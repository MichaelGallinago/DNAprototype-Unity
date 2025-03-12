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
                Vector2Int resolution = AppSettings.Options.AspectRatio.GetScaledResolution(value + 1);
                Screen.SetResolution(resolution.x, resolution.y, Screen.fullScreenMode); 
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
                
                DisplayInfo currentInfo = Screen.mainWindowDisplayInfo;
                Vector2Int resolution;
                int scale = AppSettings.Options.Scale + 1;
                do
                {
                    resolution = minResolution * scale;
                    scale--;
                } while ((resolution.x > currentInfo.width || resolution.y > currentInfo.height) && scale != 0);

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
