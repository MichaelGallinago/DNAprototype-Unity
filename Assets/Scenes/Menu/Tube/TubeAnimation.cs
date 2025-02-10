using Cysharp.Threading.Tasks;
using LitMotion;
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
        
        private void Start() => UpdateScale(_initialValue);

        public async UniTask Play()
        {
            await UniTask.WaitForSeconds(_delay);
            
            await LMotion.Create(_initialValue, _scaleTarget, _scaleDuration)
                .WithEase(Ease.OutSine)
                .Bind(UpdateScale);
        }
        
        private void UpdateScale(float value) => _image.material.SetFloat(TubeShaderProperties.ScaleId, value);
    }
}
