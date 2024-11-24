#if UNITY_EDITOR
using System.Runtime.CompilerServices;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using static Utilities.ScriptableObjectUtilities;

namespace Tiles.SizeMapStorage
{
    [CreateAssetMenu(
        fileName = nameof(SizeMapStorageScriptableObject), 
        menuName = ScriptableObjectsFolder + nameof(SizeMapStorageScriptableObject))]
    public class SizeMapStorageScriptableObject : ScriptableObject
    {
        [SerializeField, SerializedDictionary("Size maps", "Size arrays")]
        private SerializedDictionary<SizeMap<byte>, byte[]> _sizeMaps;
        
        public byte[] this[SizeMap<byte> key]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)] get
            {
                if (_sizeMaps.TryGetValue(key, out byte[] result)) return result;
                _sizeMaps.Add(key, result = key.ToArray());
                return result;
            }
        }
    }
}
#endif
