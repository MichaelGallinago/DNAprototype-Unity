using System;
using UnityEngine;

namespace DnaCore.PhysicsEcs2D.Tiles.Storage.Editor
{
    [Serializable]
    public struct SpriteStorageData
    {
        [field: SerializeField] public int Index { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        
        public SpriteStorageData(int index, Sprite sprite)
        {
            Index = index;
            Sprite = sprite;
        }
    }
}
