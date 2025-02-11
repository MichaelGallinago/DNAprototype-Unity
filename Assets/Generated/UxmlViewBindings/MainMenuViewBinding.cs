using System.Diagnostics.CodeAnalysis;
using UnityEngine.UIElements;
using System;

namespace UxmlViewBindings
{
    public readonly struct MainMenuViewBinding
    {
        [NotNull] public readonly VisualElement Root;
        [NotNull] public readonly OptionCardViewBinding CardSaves;
        [NotNull] public readonly OptionCardViewBinding CardSettings;
        [NotNull] public readonly OptionCardViewBinding CardShutdown;
        [NotNull] public readonly VisualElement Logo;

        public MainMenuViewBinding(VisualElement root)
        {
            Root = root;
            CardSaves = new OptionCardViewBinding(root.Q<VisualElement>("CardSaves") ?? throw new NullReferenceException("\"CardSaves\" not found!"));
            CardSettings = new OptionCardViewBinding(root.Q<VisualElement>("CardSettings") ?? throw new NullReferenceException("\"CardSettings\" not found!"));
            CardShutdown = new OptionCardViewBinding(root.Q<VisualElement>("CardShutdown") ?? throw new NullReferenceException("\"CardShutdown\" not found!"));
            Logo = root.Q<VisualElement>("Logo") ?? throw new NullReferenceException("\"Logo\" not found!");
        }
    }
}
