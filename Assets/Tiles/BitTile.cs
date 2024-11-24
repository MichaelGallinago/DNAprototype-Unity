using System;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;
using static System.Runtime.CompilerServices.MethodImplOptions;

[assembly: StructArrayAttributes.StructArray("SizeMap", nameof(Tiles), 16)]

namespace Tiles
{
    [Serializable]
    public struct BitTile
    {
        public const byte Size = 16;
        
        private BitArray256 _data;
        
        public bool this[int x, int y]
        {
            [MethodImpl(AggressiveInlining)] get => _data[(byte)(y << 4 + x)];
            [MethodImpl(AggressiveInlining)] set => _data[(byte)(y << 4 + x)] = value;
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
        
        public void GetSizes(out SizeDto sizeMaps)
        {
            sizeMaps = new SizeDto();
            
            for (var y = 0; y < Size; y++)
            for (var x = 0; x < Size; x++)
            {
                if (!this[x, y]) continue;
                
                if (sizeMaps.Top[y] < Size - y)
                {
                    sizeMaps.Top[y] = (byte)(Size - y);
                }

                if (sizeMaps.Bottom[y] < y)
                {
                    sizeMaps.Bottom[y] = (byte)y;
                }
                
                if (sizeMaps.Left[y] < Size - x)
                {
                    sizeMaps.Left[y] = (byte)(Size - x);
                }

                if (sizeMaps.Right[y] < x)
                {
                    sizeMaps.Right[y] = (byte)x;
                }
            }
        }
        
        public ref struct SizeDto
        {
            public SizeMap<byte> Top;
            public SizeMap<byte> Bottom;
            public SizeMap<byte> Left;
            public SizeMap<byte> Right;
        }
    }
}
