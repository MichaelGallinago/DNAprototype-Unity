using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tiles.Storage
{
    [Serializable]
    public struct FreeSpaceMap
    {
        [SerializeField] private List<int> _list;
        
        public int Take()
        {
            if (_list.Count <= 0) return -1;
            
            int endIndex = _list.Count - 1;
            int index = _list[endIndex];
            _list.RemoveAt(endIndex);
            return index;
        }
        
        public void Add(int index) => _list.Add(index);
        
        public void Clear() => _list.Clear();
    }
}
