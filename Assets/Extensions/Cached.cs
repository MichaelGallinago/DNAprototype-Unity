using System.Runtime.CompilerServices;
using UnityEngine;

namespace Extensions
{
    public struct Cached<T> where T : Object
    {
        private T _target;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator T(Cached<T> target) => target._target;
        
        public Object Target
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (_target) return;
                _target = (T)value;
            }
        }
    }
}
