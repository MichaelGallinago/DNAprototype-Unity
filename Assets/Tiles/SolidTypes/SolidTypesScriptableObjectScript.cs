using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Tiles.SolidTypes
{
    public enum SolidType : byte { Full, Top, NoTop }

    [CreateAssetMenu(fileName = "SolidTypesScriptableObjectScript", menuName = "Scriptable Objects/SolidTypesScriptableObjectScript")]
    public class SolidTypesScriptableObjectScript : ScriptableObject
    {
        [SerializedDictionary("Surrogate material key", "Solid type")]
        public SerializedDictionary<PhysicsMaterial2D, SolidType> SolidTypes;
    }
}
