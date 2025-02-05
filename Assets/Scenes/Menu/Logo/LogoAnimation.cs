using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace Scenes.Menu.Logo
{
    public class LogoAnimation : MonoBehaviour
    {
        [SerializeField] private AudioClip _audioClip;
        
        [SerializeField] private UnityEvent<AudioClip, float> _onSoundEmitted;
        
        private void Start() => _ = PlayAnimation();
        
        private async UniTask PlayAnimation()
        {
            _ = LMotion.Create(90f, 360f, 1f)
                .WithEase(Ease.InOutQuad)
                .BindToLocalEulerAnglesY(transform);
            
            await UniTask.WaitForSeconds(0.1f);
            _onSoundEmitted?.Invoke(_audioClip, 0.2f);
            await UniTask.WaitForSeconds(0.4f);
            _onSoundEmitted?.Invoke(_audioClip, 0.2f);
        }
    }
}
