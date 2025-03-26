using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DnaCore.PhysicsEcs2D.Tiles.Storage.Editor
{
    [Serializable]
    public struct FreeSpaceMap
    {
        [SerializeField] private List<int> _list;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Shrink(int maxIndex)
        {
            int count = _list.Count;

            var index1 = 0;
            while (index1 < count && _list[index1] <= maxIndex)
            {
                ++index1;
            }

            if (index1 >= count) return;

            MoveIndexes(maxIndex, index1);

            _list.RemoveRange(index1, count - index1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Take(int count)
        {
            if (_list.Count <= 0) return count;
            
            int index = _list[0];
            _list.RemoveAt(0);
            return index;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(int index)
        {
            int insertIndex = _list.BinarySearch(index);
            _list.Insert(insertIndex < 0 ? ~insertIndex : insertIndex, index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear() => _list.Clear();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void MoveIndexes(int maxIndex, int index1)
        {
            int count = _list.Count;
            int index2 = index1 + 1;
            while (index2 < count)
            {
                while (index2 < count && _list[index2] > maxIndex)
                {
                    index2++;
                }

                if (index2 < count)
                {
                    _list[index1++] = _list[index2++];
                }
            }
        }
    }
}
