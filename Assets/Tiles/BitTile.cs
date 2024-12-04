using System;
using System.Runtime.CompilerServices;
using Tiles;
using UnityEngine.Rendering;
using static System.Runtime.CompilerServices.MethodImplOptions;

[assembly: StructArrayAttributes.StructArray("SizeMap", nameof(Tiles), TileUtilities.Size)]

namespace Tiles
{
    [Serializable]
    public struct BitTile
    {
        private BitArray256 _data;
        
        public bool this[uint x, uint y]
        {
            [MethodImpl(AggressiveInlining)] get => _data[y * TileUtilities.Size + x];
            [MethodImpl(AggressiveInlining)] set => _data[y * TileUtilities.Size + x] = value;
        }
        
        public bool this[byte bitIndex]
        {
            [MethodImpl(AggressiveInlining)] get => _data[bitIndex];
            [MethodImpl(AggressiveInlining)] set => _data[bitIndex] = value;
        }
        
        public bool this[uint bitIndex]
        {
            [MethodImpl(AggressiveInlining)] get => _data[bitIndex];
            [MethodImpl(AggressiveInlining)] set => _data[bitIndex] = value;
        }
        
        public void GetSizes(out SizeDto sizeMaps)
        {
            sizeMaps = new SizeDto();
            
            for (byte y = 0; y < TileUtilities.Size; y++)
            for (byte x = 0; x < TileUtilities.Size; x++)
            {
                if (!this[x, y]) continue;
                
                if (sizeMaps.Down[x] < y + 1)
                {
                    sizeMaps.Down[x] = (byte)(y + 1);
                }
                
                if (sizeMaps.Right[y] < TileUtilities.Size - x)
                {
                    sizeMaps.Right[y] = (byte)(TileUtilities.Size - x);
                }
                
                if (sizeMaps.Up[x] < TileUtilities.Size - y)
                {
                    sizeMaps.Up[x] = (byte)(TileUtilities.Size - y);
                }
                
                if (sizeMaps.Left[y] < x + 1)
                {
                    sizeMaps.Left[y] = (byte)(x + 1);
                }
            }
        }
        
        public ref struct SizeDto
        {
            public SizeMap<byte> Down;
            public SizeMap<byte> Right;
            public SizeMap<byte> Up;
            public SizeMap<byte> Left;
        }

        public override string ToString() => _data.humanizedData;
    }
}
