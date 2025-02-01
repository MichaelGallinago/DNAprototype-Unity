using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace Scenes.Menu
{
    public class ModelAnimation : MonoBehaviour
    {
        [SerializeField] private float _rotationDuration = 15f;
        [SerializeField] private float _snappingDuration = 15f;
        [SerializeField] private MeshRenderer _meshRenderer;

        private void Start()
        {
            LMotion.Create(0f, WireframeShaderProperties.SnapMaximum, _snappingDuration).Bind(UpdateSnapping);
            LMotion.Create(0f, 360f, _rotationDuration).WithLoops(-1).BindToLocalEulerAnglesY(transform);
        }
        
        private void UpdateSnapping(float value)
        {
            value /= WireframeShaderProperties.SnapMaximum;
            value *= value * WireframeShaderProperties.SnapMaximum;
            _meshRenderer.material.SetFloat(WireframeShaderProperties.SnapId, value);
        }
    }
}
