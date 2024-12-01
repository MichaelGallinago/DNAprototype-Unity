using System;
using Tiles.Generator;
using UnityEngine;

namespace Tiles.Storage
{
    [Serializable]
    public struct TileStorageData
    {
        public int Count;
        [field: SerializeField] public int Index { get; private set; }
        [field: SerializeField] public GeneratedTile Tile { get; private set; }

        public TileStorageData(int count, int index, GeneratedTile tile)
        {
            Count = count;
            Index = index;
            Tile = tile;
        }
    }
}
