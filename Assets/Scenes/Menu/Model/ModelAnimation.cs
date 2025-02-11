using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using Scenes.Menu.Wireframe;
using UnityEngine;

namespace Scenes.Menu.Model
{
    public class ModelAnimation : MonoBehaviour
    {
        [SerializeField] private float _rotationDuration = 15f;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Transform _meshTransform;

        private float _snap;
        
        private void Start() => UpdateSnapping(_snap);
        
        public async UniTask PlayAppearance(float duration) => 
            await StartSnapChanging(_snap, WireframeShaderProperties.SnapMaximum, duration, Ease.InQuad);
        
        public async UniTask PlayDisappearance(float duration) => 
            await StartSnapChanging(_snap, 0f, duration, Ease.OutQuad);
        
        public async UniTask PlayRotation()
        {
            await LMotion.Create(0f, -30f, _rotationDuration / 4f)
                .WithEase(Ease.InOutQuad)
                .BindToLocalEulerAnglesY(_meshTransform);
            
            await LMotion.Create(-30f, 360f, _rotationDuration * 1.5f)
                .WithEase(Ease.InSine)
                .BindToLocalEulerAnglesY(_meshTransform);
            
            await LMotion.Create(0f, 360f, _rotationDuration).WithLoops(-1).BindToLocalEulerAnglesY(_meshTransform);
        }

        private async UniTask StartSnapChanging(float from, float to, float duration, Ease ease) =>
            await LMotion.Create(from, to, duration)
                .WithEase(ease)
                .Bind(UpdateSnapping);
        
        private void UpdateSnapping(float value)
        {
            _snap = value;
            value /= WireframeShaderProperties.SnapMaximum;
            value *= value * WireframeShaderProperties.SnapMaximum;
            _meshRenderer.material.SetFloat(WireframeShaderProperties.SnapId, value);
        }
    }
}
