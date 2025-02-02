using System;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace Scenes.Menu.Card
{
    public class CardAnimation : MonoBehaviour
    {
        [SerializeField] private float _appearanceDuration = 2f;
        [SerializeField] private float _offset = 256f;
        [SerializeField] private AudioClip _audioClip;
        [SerializeField] private MenuAudioSource _audioSource;

        private void Start() => _ = PlayAnimation();

        private async UniTask PlayAnimation()
        {
            LMotion.Create(transform.position.x - _offset, transform.position.x, _appearanceDuration)
                .WithEase(Ease.InOutBack)
                .BindToPositionX(transform);
            
            await UniTask.WaitForSeconds(MathF.Max(_appearanceDuration - 1f, 0f));
            
            _audioSource.Play(_audioClip, 0.1f);
        }
    }
}
