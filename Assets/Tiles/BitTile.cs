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
        
        public void GetSizes(
            BitTile tile, out (SizeMap<byte> top, SizeMap<byte> bottom, SizeMap<byte> left, SizeMap<byte> right) sizes)
        {
            var heightsTop = new SizeMap<byte>();
            var heightsBottom = new SizeMap<byte>();
            var widthsLeft = new SizeMap<byte>();
            var widthsRight = new SizeMap<byte>();
            
            for (var y = 0; y < Size; y++)
            for (var x = 0; x < Size; x++)
            {
                if (!tile[x, y]) continue;
                
                if (heightsTop[y] < Size - y)
                {
                    heightsTop[y] = (byte)(Size - y);
                }

                if (heightsBottom[y] < y)
                {
                    heightsBottom[y] = (byte)y;
                }
                
                if (widthsLeft[y] < Size - x)
                {
                    widthsLeft[y] = (byte)(Size - x);
                }

                if (widthsRight[y] < x)
                {
                    widthsRight[y] = (byte)x;
                }
            }
            
            sizes = (heightsTop, heightsBottom, widthsLeft, widthsRight);
        }
    }
}
