using System;
using System.Runtime.CompilerServices;
using DnaCore.PhysicsEcs2D.Tiles;
using DnaCore.PhysicsEcs2D.Tiles.SolidTypes;
using Tiles;
using UnityEngine;
using UnityEngine.Rendering;
using static System.Runtime.CompilerServices.MethodImplOptions;

[assembly: StructArrayAttributes.StructArray("SizeMap", nameof(Tiles), TileConstants.Size)]

namespace DnaCore.PhysicsEcs2D.Tiles.Generators.Editor
{
    [Serializable]
    public struct BitTile
    {
        [SerializeField] private BitArray256 _data;
        
        public bool IsEmpty => _data == default;
        
        public bool this[uint x, uint y]
        {
            [MethodImpl(AggressiveInlining)] get => _data[y * TileConstants.Size + x];
            [MethodImpl(AggressiveInlining)] set => _data[y * TileConstants.Size + x] = value;
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
        
        public override string ToString() => _data.humanizedData;

        public void GetSizes(SolidType solidType, out SizeDto sizeMaps)
        {
            switch(solidType)
            {
                case SolidType.Full: GetFullSizes(out sizeMaps); break;
                case SolidType.Top: GetTopSizes(out sizeMaps); break;
                case SolidType.NoTop: GetNoTopSizes(out sizeMaps); break;
                default: GetFullSizes(out sizeMaps); break;
            }
        }

        [MethodImpl(AggressiveInlining)]
        private void GetFullSizes(out SizeDto sizeMaps)
        {
            sizeMaps = new SizeDto();
            
            for (byte y = 0; y < TileConstants.Size; y++)
            for (byte x = 0; x < TileConstants.Size; x++)
            {
                if (!this[x, y]) continue;
                
                if (sizeMaps.Down[x] < y + 1)
                {
                    sizeMaps.Down[x] = (byte)(y + 1);
                }
                
                if (sizeMaps.Right[y] < TileConstants.Size - x)
                {
                    sizeMaps.Right[y] = (byte)(TileConstants.Size - x);
                }
                
                if (sizeMaps.Up[x] < TileConstants.Size - y)
                {
                    sizeMaps.Up[x] = (byte)(TileConstants.Size - y);
                }
                
                if (sizeMaps.Left[y] < x + 1)
                {
                    sizeMaps.Left[y] = (byte)(x + 1);
                }
            }
        }
        
        [MethodImpl(AggressiveInlining)]
        private void GetTopSizes(out SizeDto sizeMaps)
        {
            sizeMaps = new SizeDto();
            
            for (byte y = 0; y < TileConstants.Size; y++)
            for (byte x = 0; x < TileConstants.Size; x++)
            {
                if (!this[x, y]) continue;
                
                if (sizeMaps.Down[x] < y + 1)
                {
                    sizeMaps.Down[x] = (byte)(y + 1);
                }
            }
        }
        
        [MethodImpl(AggressiveInlining)]
        private void GetNoTopSizes(out SizeDto sizeMaps)
        {
            sizeMaps = new SizeDto();
            
            for (byte y = 0; y < TileConstants.Size; y++)
            for (byte x = 0; x < TileConstants.Size; x++)
            {
                if (!this[x, y]) continue;
                
                if (sizeMaps.Right[y] < TileConstants.Size - x)
                {
                    sizeMaps.Right[y] = (byte)(TileConstants.Size - x);
                }
                
                if (sizeMaps.Up[x] < TileConstants.Size - y)
                {
                    sizeMaps.Up[x] = (byte)(TileConstants.Size - y);
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
    }
}
