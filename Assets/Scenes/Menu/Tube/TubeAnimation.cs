using System;
using Cysharp.Threading.Tasks;
using LitMotion;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scenes.Menu.Tube
{
    public class TubeAnimation : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private float _scaleDuration = 5f;
        [SerializeField] private float _scaleTarget = 1f;
        [SerializeField] private float _initialValue;
        [SerializeField] private float _delay;
        [SerializeField] private AudioClip _audioClip;
        
        [SerializeField] private UnityEvent<AudioClip, float> _onSoundEmitted;
        [SerializeField] private UnityEvent _onAnimationFinished;
        
        private void Start() => _ = PlayAnimation();
        
        private async UniTask PlayAnimation()
        {
            UpdateScale(_initialValue);
            
            await UniTask.WaitForSeconds(_delay);
            
            _ = LMotion.Create(_initialValue, _scaleTarget, _scaleDuration)
                .WithEase(Ease.OutSine)
                .Bind(UpdateScale);
            
            await UniTask.WaitForSeconds(0.5f);
            _onSoundEmitted?.Invoke(_audioClip, 0.5f);
            
            await UniTask.WaitForSeconds(MathF.Max(_audioClip.length - 1f, 0f));

            _onAnimationFinished?.Invoke();
        }
        
        private void UpdateScale(float value) => _image.material.SetFloat(TubeShaderProperties.ScaleId, value);
    }
}
