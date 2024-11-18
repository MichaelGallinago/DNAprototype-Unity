using System;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;
using static System.Runtime.CompilerServices.MethodImplOptions;

namespace Tiles
{
    [Serializable]
    public struct BitTile
    {
        private BitArray256 _data;
        
        public bool this[int row, int col]
        {
            [MethodImpl(AggressiveInlining)] get => _data[(byte)(row << 4 + col)];
            [MethodImpl(AggressiveInlining)] set => _data[(byte)(row << 4 + col)] = value;
        }
        
        public bool this[byte bitIndex]
        {
            [MethodImpl(AggressiveInlining)] get => _data[bitIndex];
            [MethodImpl(AggressiveInlining)] set => _data[bitIndex] = value;
        }
        
        public bool this[int bitIndex]
        {
            [MethodImpl(AggressiveInlining)] get => _data[(byte)bitIndex];
            [MethodImpl(AggressiveInlining)] set => _data[(byte)bitIndex] = value;
        }
        
        [MethodImpl(AggressiveInlining)] public bool Equals(BitTile other) => _data == other._data;
    }
}
