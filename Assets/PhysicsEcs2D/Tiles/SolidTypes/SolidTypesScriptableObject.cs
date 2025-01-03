#if UNITY_EDITOR
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using static Utilities.ScriptableObjectUtilities;

namespace Tiles.SolidTypes
{
    [CreateAssetMenu(
        fileName = nameof(SolidTypesScriptableObject), 
        menuName = Folder + nameof(SolidTypesScriptableObject))]
    public class SolidTypesScriptableObject : ScriptableObject
    {
        [SerializeField, SerializedDictionary("Surrogate material key", nameof(SolidType))]
        private SerializedDictionary<PhysicsMaterial2D, SolidType> _solidTypes;

        public SolidType this[PhysicsMaterial2D material]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)] get => _solidTypes[material];
        }
        
        public PhysicsMaterial2D this[SolidType type]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)] get
            {
                foreach (KeyValuePair<PhysicsMaterial2D, SolidType> pair in _solidTypes)
                {
                    if (pair.Value == type) return pair.Key;
                }
                
                return null;
            }
        }
    }
}
#endif
