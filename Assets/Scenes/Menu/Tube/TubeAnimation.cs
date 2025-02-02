using System;
using Cysharp.Threading.Tasks;
using LitMotion;
using Scenes.Menu.Audio;
using UnityEngine;
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
        [SerializeField] private MenuAudioSource _audioSource;
        [SerializeField] private MenuThemeAudioSource _menuThemeAudioSource;
        
        private void Start() => _ = PlayAnimation();
        
        private async UniTask PlayAnimation()
        {
            UpdateScale(_initialValue);
            
            await UniTask.WaitForSeconds(_delay);
            
            LMotion.Create(_initialValue, _scaleTarget, _scaleDuration)
                .WithEase(Ease.OutSine)
                .Bind(UpdateScale);
            
            await UniTask.WaitForSeconds(0.5f);
            _audioSource.Play(_audioClip, 0.5f);
            
            await UniTask.WaitForSeconds(MathF.Max(_audioClip.length - 1f, 0f));
            _menuThemeAudioSource.PlayTheme();
        }
        
        private void UpdateScale(float value) => _image.material.SetFloat(TubeShaderProperties.ScaleId, value);
    }
}
