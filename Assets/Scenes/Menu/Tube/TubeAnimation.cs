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
        
        private void Start() => LMotion.Create(_initialValue, _scaleTarget, _scaleDuration)
            .WithEase(Ease.OutSine)
            .Bind(UpdateScale);

        private void UpdateScale(float value) => _image.material.SetFloat(TubeShaderProperties.ScaleId, value);
    }
}
