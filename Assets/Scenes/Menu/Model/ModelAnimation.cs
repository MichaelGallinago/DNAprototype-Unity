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
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private AudioClip _audioClip;
        [SerializeField] private MenuAudioSource _audioSource;

        private void Start() => _ = PlayAnimation();

        public async UniTask StartRotation()
        {
            await LMotion.Create(0f, 60f, _rotationDuration)
                .WithEase(Ease.InBack)
                .BindToLocalEulerAnglesY(transform);
            
            await LMotion.Create(60f, 360f, _rotationDuration).BindToLocalEulerAnglesY(transform);
            
            LMotion.Create(0f, 360f, _rotationDuration).WithLoops(-1).BindToLocalEulerAnglesY(transform);
        }
        
        private async UniTask PlayAnimation()
        {
            UpdateSnapping(0f);
            
            await UniTask.WaitForSeconds(_delay);
            
            LMotion.Create(0f, WireframeShaderProperties.SnapMaximum, _snappingDuration)
                .WithEase(Ease.InQuad)
                .Bind(UpdateSnapping);
            
            await UniTask.WaitForSeconds(1.2f);
            _audioSource.Play(_audioClip, 0.5f);
        }
        
        private void UpdateSnapping(float value)
        {
            value /= WireframeShaderProperties.SnapMaximum;
            value *= value * WireframeShaderProperties.SnapMaximum;
            _meshRenderer.material.SetFloat(WireframeShaderProperties.SnapId, value);
        }
    }
}
