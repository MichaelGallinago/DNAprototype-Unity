using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace Scenes.Menu.Logo
{
    public class LogoAnimation : MonoBehaviour
    {
        [SerializeField] private AudioClip _audioClip;
        [SerializeField] private MenuAudioSource _audioSource;
        
        private void Start() => _ = PlayAnimation();
        
        private async UniTask PlayAnimation()
        {
            LMotion.Create(90f, 360f, 1f)
                .WithEase(Ease.InOutQuad)
                .BindToLocalEulerAnglesY(transform);
            
            await UniTask.WaitForSeconds(0.1f);
            _audioSource.Play(_audioClip, 0.2f);
            await UniTask.WaitForSeconds(0.4f);
            _audioSource.Play(_audioClip, 0.2f);
        }
    }
}
