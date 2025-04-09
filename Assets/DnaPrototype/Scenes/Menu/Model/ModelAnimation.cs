using DnaPrototype.Scenes.Menu.Wireframe;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace Scenes.Menu.Model
{
    public class ModelAnimation : MonoBehaviour
    {
        [SerializeField] private float _rotationDuration = 15f;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Transform _meshTransform;

        private float _snap;
        
        private void Start() => UpdateSnapping(0f, this);
        
        public MotionHandle PlayAppearance(float duration) => 
            StartSnapChanging(_snap, WireframeShaderProperties.SnapMaximum, duration, Ease.InQuad);
         
        public MotionHandle PlayDisappearance(float duration) => 
            StartSnapChanging(_snap, 0f, duration, Ease.OutQuad);
        
        public MotionHandle PlayRotation() => LSequence.Create()
            .Append(LMotion.Create(0f, -30f, _rotationDuration / 4f)
                .WithEase(Ease.InOutQuad)
                .BindToLocalEulerAnglesY(_meshTransform))
            .Append(LMotion.Create(-30f, 360f, _rotationDuration * 1.5f)
                .WithEase(Ease.InSine)
                .BindToLocalEulerAnglesY(_meshTransform))
            .Run();
        
        public MotionHandle LoopRotation() => LMotion.Create(0f, 360f, _rotationDuration)
            .WithLoops(-1)
            .BindToLocalEulerAnglesY(_meshTransform);

        private MotionHandle StartSnapChanging(float from, float to, float duration, Ease ease) =>
            LMotion.Create(from, to, duration)
                .WithEase(ease)
                .Bind(this, static (snap, model) => UpdateSnapping(snap, model));
        
        private static void UpdateSnapping(float value, ModelAnimation modelAnimation)
        {
            modelAnimation._snap = value;
            value /= WireframeShaderProperties.SnapMaximum;
            value *= value * WireframeShaderProperties.SnapMaximum;
            modelAnimation._meshRenderer.material.SetFloat(WireframeShaderProperties.SnapId, value);
        }
    }
}
