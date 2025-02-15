using System.Diagnostics.CodeAnalysis;
using UnityEngine.UIElements;
using System;

namespace UxmlViewBindings
{
    public readonly struct SettingsViewBinding
    {
        [NotNull] public readonly VisualElement Root;
        [NotNull] public readonly ScrollView Options;
        [NotNull] public readonly ScrollView Control;

        public SettingsViewBinding(VisualElement root)
        {
            Root = root;
            Options = root.Q<ScrollView>("Options") ?? throw new NullReferenceException("\"Options\" not found!");
            Control = root.Q<ScrollView>("Control") ?? throw new NullReferenceException("\"Control\" not found!");
        }
    }
}
