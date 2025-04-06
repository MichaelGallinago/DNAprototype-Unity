using DnaCore.Singletons;
using DnaCore.Utilities;
using UnityEngine;

namespace DnaCore.Editor.DebugRender
{
    [CreateAssetMenu(
        fileName = nameof(CollisionSensorsDebugSystemAssets), 
        menuName = AssetMenuPaths.SystemAssets + nameof(CollisionSensorsDebugSystem))]
    public class CollisionSensorsDebugSystemAssets : ScriptableSingleton<CollisionSensorsDebugSystemAssets>
    {
        [field:SerializeField] public Material Material { get; private set; }
        [field:SerializeField] public Mesh Mesh { get; private set; }
    }
}
