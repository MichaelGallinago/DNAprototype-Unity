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
        
        public MotionHandle PlayAppear(float duration) => StartScaleAnimation(_scale, _scaleTarget, duration);
        public MotionHandle PlayHide(float duration) => StartScaleAnimation(_scale, 0f, duration);

        private MotionHandle StartScaleAnimation(float from, float to, float duration)
        {
            if (_animation.IsPlaying())
            {
                _animation.Cancel();
            }
            
            return _animation = LMotion.Create(from, to, duration).WithEase(Ease.OutSine).Bind(UpdateScale);
        }
        
        private void UpdateScale(float value) => _image.material.SetFloat(TubeShaderProperties.ScaleId, _scale = value);
    }
}
