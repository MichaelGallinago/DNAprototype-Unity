using System;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsEcs2D.Tiles.Storage.Editor
{
    [Serializable]
    public struct FreeSpaceMap
    {
        [SerializeField] private List<int> _list;

        public void Shrink(int maxIndex) => _list.RemoveAll(n => n > maxIndex);

        public int Take(int count)
        {
            if (_list.Count <= 0) return count;
            
            int index = _list[0];
            _list.RemoveAt(0);
            return index;
        }
        
        public void Add(int index)
        {
            int insertIndex = _list.BinarySearch(index);
            _list.Insert(insertIndex < 0 ? ~insertIndex : insertIndex, index);
        }

        public void Clear() => _list.Clear();
    }
}
