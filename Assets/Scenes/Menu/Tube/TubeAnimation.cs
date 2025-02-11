using Cysharp.Threading.Tasks;
using LitMotion;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.Menu.Tube
{
    public class TubeAnimation : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private float _scaleTarget = 1f;
        
        private MotionHandle _animation;
        private float _scale;
        
        private void Start() => UpdateScale(_scale);
        
        public async UniTask PlayAppear(float duration) => await StartScaleAnimation(_scale, _scaleTarget, duration);
        public async UniTask PlayHide(float duration) => await StartScaleAnimation(_scale, 0f, duration);

        private async UniTask StartScaleAnimation(float from, float to, float duration)
        {
            if (_animation.IsPlaying())
            {
                _animation.Cancel();
            }
            
            await (_animation = LMotion.Create(from, to, duration).WithEase(Ease.OutSine).Bind(UpdateScale));
        }
        
        private void UpdateScale(float value) => _image.material.SetFloat(TubeShaderProperties.ScaleId, _scale = value);
    }
}
