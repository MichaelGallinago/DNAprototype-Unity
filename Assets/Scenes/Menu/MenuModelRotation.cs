using UnityEngine;

namespace Scenes.Menu
{
    public class MenuModelRotation : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 15f;
        [SerializeField] private float _snappingUpdateSpeed = 3f;
        [SerializeField] private MeshRenderer _meshRenderer;

        private float _time;
        
        private void Update()
        {
            UpdateSnapping();
            RotateModel();
        }

        private void UpdateSnapping()
        {
            if (_time >= WireframeShaderProperties.SnapMaximum) return;
            
            float maxDelta = _snappingUpdateSpeed * Time.deltaTime;
            _time = Mathf.MoveTowards(_time, WireframeShaderProperties.SnapMaximum, maxDelta);
            
            float value = _time / WireframeShaderProperties.SnapMaximum;
            value *= value * WireframeShaderProperties.SnapMaximum;
            
            _meshRenderer.material.SetFloat(WireframeShaderProperties.SnapId, value);
        }

        private void RotateModel() => transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
    }
}
