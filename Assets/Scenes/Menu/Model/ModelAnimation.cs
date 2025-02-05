using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using Scenes.Menu.Wireframe;
using UnityEngine;
using UnityEngine.Events;

namespace Scenes.Menu.Model
{
    public class ModelAnimation : MonoBehaviour
    {
        [SerializeField] private float _rotationDuration = 15f;
        [SerializeField] private float _snappingDuration = 15f;
        [SerializeField] private float _delay = 5f;
        [SerializeField] private float _rotationDelay = 1f;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private AudioClip _audioClip;
        
        [SerializeField] private UnityEvent<AudioClip, float> _onSoundEmitted;

        private void Start() => _ = PlayAnimation();
        
        public void StartRotation() => _ = Rotate();

        private async UniTask PlayAnimation()
        {
            UpdateSnapping(0f);
            
            await UniTask.WaitForSeconds(_delay);
            
            _ = LMotion.Create(0f, WireframeShaderProperties.SnapMaximum, _snappingDuration)
                .WithEase(Ease.InQuad)
                .Bind(UpdateSnapping);
            
            await UniTask.WaitForSeconds(1.2f);
            _onSoundEmitted?.Invoke(_audioClip, 0.5f);
        }

        private async UniTask Rotate()
        {
            await UniTask.WaitForSeconds(_rotationDelay);
            
            await LMotion.Create(0f, -30f, _rotationDuration / 4f)
                .WithEase(Ease.InOutQuad)
                .BindToLocalEulerAnglesY(transform);
            
            await LMotion.Create(-30f, 360f, _rotationDuration * 1.5f)
                .WithEase(Ease.InSine)
                .BindToLocalEulerAnglesY(transform);
            
            _ = LMotion.Create(0f, 360f, _rotationDuration).WithLoops(-1).BindToLocalEulerAnglesY(transform);
        }
        
        private void UpdateSnapping(float value)
        {
            value /= WireframeShaderProperties.SnapMaximum;
            value *= value * WireframeShaderProperties.SnapMaximum;
            _meshRenderer.material.SetFloat(WireframeShaderProperties.SnapId, value);
        }
    }
}
