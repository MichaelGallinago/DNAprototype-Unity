#if UNITY_EDITOR
using System;
using System.Runtime.CompilerServices;
using AYellowpaper.SerializedCollections;
using UnityEditor;
using UnityEngine;
using Utilities;
using static Utilities.ScriptableObjectUtilities;

namespace Tiles.Storage
{
    [CreateAssetMenu(
        fileName = nameof(SizeDataStorageScriptableObject), 
        menuName = ScriptableObjectsFolder + nameof(SizeDataStorageScriptableObject))]
    public class SizeDataStorageScriptableObject : ScriptableObject
    {
        private static readonly Vector4 RightAngles = new(0f, Circle.Quarter, Circle.Half, Circle.HalfAndQuarter);
        
        [SerializeField, SerializedDictionary(nameof(SizeMap<byte>), nameof(SizeData))]
        private SerializedDictionary<SizeMap<byte>, SizeData> _sizeMaps;
        
        public SizeData this[SizeMap<byte> key]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)] get
            {
                if (_sizeMaps.TryGetValue(key, out SizeData result)) return result;
                _sizeMaps.Add(key, result = new SizeData(key.ToArray(), CalculateAngles(key)));
                return result;
            }
        }
        
        public void Clear()
        {
            _sizeMaps.Clear();
            AssetDatabaseUtilities.SetDirtyAndSave(this);
        }

        private static Vector4 CalculateAngles(SizeMap<byte> sizes)
        {
            SearchIndexes indexes = CalculateIndexes(sizes);
            
            if (indexes.Second == byte.MaxValue) return RightAngles;
            
            bool isIncreasesOnStart = sizes[indexes.Second] > sizes[indexes.First];
            bool isIncreasesOnEnd = sizes[indexes.Last] > sizes[indexes.Previous];
            
            int index1 = isIncreasesOnStart ? indexes.First : indexes.Second - 1;
            int index2 = isIncreasesOnEnd ? indexes.Previous + 1 : indexes.Last;
            
            double angle = Math.Atan2(sizes[index2] - sizes[index1], Math.Abs(index2 - index1));
            return GetRotatedAngles(angle);
        }
        
        private static Vector4 GetRotatedAngles(double angle) => new(
            (float)((angle + CircleDouble.Full) % CircleDouble.Full), 
            (float)((angle + CircleDouble.FullAndQuarter) % CircleDouble.Full),
            (float)((CircleDouble.Half - angle) % CircleDouble.Full),
            (float)((CircleDouble.HalfAndQuarter - angle) % CircleDouble.Full)
        );

        private static SearchIndexes CalculateIndexes(SizeMap<byte> sizes)
        {
            var indexes = new SearchIndexes
            {
                First = byte.MaxValue,
                Second = byte.MaxValue,
            };
            
            for (byte i = 0; i < SizeMap<byte>.Length; i++)
            {
                byte size = sizes[i];
                if (size == 0) continue;

                if (sizes[indexes.Last] != size)
                {
                    indexes.Previous = indexes.Last;
                }
                
                indexes.Last = i;
                
                if (indexes.First == byte.MaxValue)
                {
                    indexes.First = i;
                }
                else if (size != sizes[indexes.First] && indexes.Second == byte.MaxValue)
                {
                    indexes.Second = i;
                }
            }
            
            return indexes;
        }

        private ref struct SearchIndexes
        {
            public byte First;
            public byte Second;
            public byte Previous;
            public byte Last;
        }
    }
}
#endif
