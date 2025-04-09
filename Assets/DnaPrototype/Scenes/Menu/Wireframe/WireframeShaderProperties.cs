using UnityEngine;

namespace DnaPrototype.Scenes.Menu.Wireframe
{
    public static class WireframeShaderProperties
    {
        public const float SnapMaximum = 1000f;
        public static readonly int SnapId = Shader.PropertyToID("_Snap");
    }
}
