using DnaCore.Singletons.Window;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace DnaCore.CameraEcs
{
    [RequireComponent(typeof(Camera)), RequireComponent(typeof(PixelPerfectCamera))]
    public class CameraTracker : MonoBehaviour
    {
        [SerializeField, HideInInspector] private Camera _camera;
        [SerializeField, HideInInspector] private PixelPerfectCamera _pixelPerfectCamera;

        private void Awake() => WindowController.AddCamera(_camera, _pixelPerfectCamera);

#if UNITY_EDITOR
        private void OnValidate()
        {
            _camera = GetComponent<Camera>();
            _pixelPerfectCamera = GetComponent<PixelPerfectCamera>();
        }
#endif
    }
}
