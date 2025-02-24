using System.Diagnostics.CodeAnalysis;
using UnityEngine.UIElements;
using System;
// ReSharper disable CheckNamespace
// ReSharper disable UnusedType.Global
// ReSharper disable NotAccessedField.Global
// ReSharper disable RedundantNullableFlowAttribute

namespace UxmlViewBindings
{
    public readonly struct MainMenuViewBinding
    {
        [NotNull] public readonly VisualElement Root;
        [NotNull] public readonly VisualElement VisualElement;
        [NotNull] public readonly OptionCardViewBinding CardSaves;
        [NotNull] public readonly OptionCardViewBinding CardSettings;
        [NotNull] public readonly OptionCardViewBinding CardShutdown;
        [NotNull] public readonly SettingsViewBinding Settings;
        [NotNull] public readonly VisualElement Logo;

        public MainMenuViewBinding(VisualElement root)
        {
            Root = root;
            VisualElement = root.Q<VisualElement>("VisualElement") ?? throw new NullReferenceException("\"VisualElement\" not found!");
            CardSaves = new OptionCardViewBinding(root.Q<VisualElement>("CardSaves") ?? throw new NullReferenceException("\"CardSaves\" not found!"));
            CardSettings = new OptionCardViewBinding(root.Q<VisualElement>("CardSettings") ?? throw new NullReferenceException("\"CardSettings\" not found!"));
            CardShutdown = new OptionCardViewBinding(root.Q<VisualElement>("CardShutdown") ?? throw new NullReferenceException("\"CardShutdown\" not found!"));
            Settings = new SettingsViewBinding(root.Q<VisualElement>("Settings") ?? throw new NullReferenceException("\"Settings\" not found!"));
            Logo = root.Q<VisualElement>("Logo") ?? throw new NullReferenceException("\"Logo\" not found!");
        }
    }
}
