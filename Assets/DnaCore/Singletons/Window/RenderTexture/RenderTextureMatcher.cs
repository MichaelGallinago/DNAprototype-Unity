using DnaCore.Settings;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace DnaCore.Singletons.Window.RenderTexture
{
    public class RenderTextureMatcher : MonoBehaviour
    {
        [SerializeField] private UnityEngine.RenderTexture _renderTexture;
        [SerializeField] private TextureDepths depth = TextureDepths.None;
        
        private GraphicsFormat Format => SystemInfo.GetCompatibleFormat(
            GraphicsFormatUtility.GetGraphicsFormat(
                RenderTextureFormat.Default, 
                GraphicsFormatUtility.IsSRGBFormat(_renderTexture.graphicsFormat)
            ), GraphicsFormatUsage.Render
        );
        
        private void Start()
        {
            if (CheckRequirements()) return;
            MatchRenderTexture(AppSettings.Options.AspectRatio.MinResolution);
        }
        
        public void MatchRenderTexture(Vector2Int resolution)
        {
            if (!_renderTexture) return;
            
            _renderTexture.Release();
            SetTextureParameters(resolution);
            _renderTexture.Create();
        }

        private void SetTextureParameters(Vector2Int resolution)
        {
            _renderTexture.width = resolution.x;
            _renderTexture.height = resolution.y;
            if (depth != TextureDepths.None)
            {
                _renderTexture.depth = (int)depth;
            }
            
            RenderTextureDescriptor descriptor = _renderTexture.descriptor;
            descriptor.colorFormat = RenderTextureFormat.Default;
            _renderTexture.descriptor = descriptor;
        }

        private bool CheckRequirements()
        {
            if (depth != TextureDepths.None)
            {
                if (_renderTexture.depth != (int)depth) return false;
            }
            
            if (_renderTexture.graphicsFormat == Format) return false;
            
            Vector2Int resolution = AppSettings.Options.AspectRatio.MinResolution;
            return _renderTexture.width != resolution.x || _renderTexture.height != resolution.y;
        }
    }
}
