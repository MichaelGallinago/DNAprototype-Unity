using System;
using UnityEngine;

namespace Tiles.Generators.Editor
{
    [Serializable]
    public struct VisibilitySwitcher
    {
        public Action<bool> OnVisibilityChanged { private get; set; }
        
        public bool IsTilesVisible
        {
            get => _isTilesVisible;
            set => OnVisibilityChanged(_isTilesVisible = value);
        }
        [SerializeField] private bool _isTilesVisible;

        public void Switch() => IsTilesVisible = !_isTilesVisible;
    }
}