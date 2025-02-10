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
        [SerializeField] private float _snappingDuration = 15f;
        [SerializeField] private float _delay = 5f;
        [SerializeField] private float _rotationDelay = 1f;
        [SerializeField] private MeshRenderer _meshRenderer;

        private void Start() => UpdateSnapping(0f);
        
        public async UniTask PlayAppearance()
        {
            await UniTask.WaitForSeconds(_delay);
            
            await LMotion.Create(0f, WireframeShaderProperties.SnapMaximum, _snappingDuration)
                .WithEase(Ease.InQuad)
                .Bind(UpdateSnapping);
        }

        public async UniTask PlayRotation()
        {
            await UniTask.WaitForSeconds(_rotationDelay);
            
            await LMotion.Create(0f, -30f, _rotationDuration / 4f)
                .WithEase(Ease.InOutQuad)
                .BindToLocalEulerAnglesY(transform);
            
            await LMotion.Create(-30f, 360f, _rotationDuration * 1.5f)
                .WithEase(Ease.InSine)
                .BindToLocalEulerAnglesY(transform);
            
            await LMotion.Create(0f, 360f, _rotationDuration).WithLoops(-1).BindToLocalEulerAnglesY(transform);
        }
        
        private void UpdateSnapping(float value)
        {
            value /= WireframeShaderProperties.SnapMaximum;
            value *= value * WireframeShaderProperties.SnapMaximum;
            _meshRenderer.material.SetFloat(WireframeShaderProperties.SnapId, value);
        }
    }
}
