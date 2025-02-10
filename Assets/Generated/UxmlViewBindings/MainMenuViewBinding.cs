using System.Diagnostics.CodeAnalysis;
using UnityEngine.UIElements;
using System;

namespace UxmlViewBindings
{
    public readonly struct MainMenuViewBinding
    {
        [NotNull] public readonly VisualElement Root;
        [NotNull] public readonly OptionCardViewBinding OptionSaves;
        [NotNull] public readonly OptionCardViewBinding OptionSettings;
        [NotNull] public readonly OptionCardViewBinding OptionShutdown;
        [NotNull] public readonly VisualElement Logo;

        public MainMenuViewBinding(VisualElement root)
        {
            Root = root;
            OptionSaves = new OptionCardViewBinding(root.Q<VisualElement>("OptionSaves") ?? throw new NullReferenceException("\"OptionSaves\" not found!"));
            OptionSettings = new OptionCardViewBinding(root.Q<VisualElement>("OptionSettings") ?? throw new NullReferenceException("\"OptionSettings\" not found!"));
            OptionShutdown = new OptionCardViewBinding(root.Q<VisualElement>("OptionShutdown") ?? throw new NullReferenceException("\"OptionShutdown\" not found!"));
            Logo = root.Q<VisualElement>("Logo") ?? throw new NullReferenceException("\"Logo\" not found!");
        }
    }
}
