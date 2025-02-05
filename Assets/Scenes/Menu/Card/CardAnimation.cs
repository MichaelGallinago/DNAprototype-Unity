using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace Scenes.Menu.Card
{
    public class CardAnimation : MonoBehaviour
    {
        [SerializeField] private float _appearanceDuration = 2f;
        [SerializeField] private float _appearanceDelay;
        [SerializeField] private float _offset = 256f;
        [SerializeField] private AudioClip _audioClip;
        
        [SerializeField] private UnityEvent<AudioClip, float> _onSoundEmitted;
        [SerializeField] private UnityEvent _onInteractivityDisabled;
        [SerializeField] private UnityEvent _onInteractivityEnabled;
        
        private float _initialPosition;
        
        private void Start()
        {
            Vector3 position = transform.position;
            _initialPosition = position.x;
            position.x -= _offset;
            transform.position = position;
            
            Show(_appearanceDelay);
        }

        public void Show(float delay) => _ = PlayMovementAnimation(
            delay, _initialPosition - _offset, _initialPosition, _appearanceDuration - 1f);
        
        public void Hide(float delay) => _ = PlayMovementAnimation(
            delay, _initialPosition, _initialPosition - _offset, 0.1f);
        
        private async UniTask PlayMovementAnimation(float delay, float from, float to, float soundDelay)
        {
            await UniTask.WaitForSeconds(delay);
            
            _onInteractivityDisabled?.Invoke();
            MotionHandle appearanceHandle = LMotion.Create(from, to, _appearanceDuration)
                .WithEase(Ease.InOutBack)
                .BindToPositionX(transform);
            
            await UniTask.WaitForSeconds(Mathf.Clamp(soundDelay, 0f, _appearanceDuration));
            _onSoundEmitted?.Invoke(_audioClip, 0.1f);

            await appearanceHandle;
            _onInteractivityEnabled?.Invoke();
        }
    }
}
