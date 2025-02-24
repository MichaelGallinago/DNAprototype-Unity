using System.Diagnostics.CodeAnalysis;
using UnityEngine.UIElements;
using System;
// ReSharper disable CheckNamespace
// ReSharper disable UnusedType.Global
// ReSharper disable NotAccessedField.Global
// ReSharper disable RedundantNullableFlowAttribute

namespace UxmlViewBindings
{
    public readonly struct SettingsViewBinding
    {
        [NotNull] public readonly VisualElement Root;
        [NotNull] public readonly ScrollView Options;
        [NotNull] public readonly CustomSliderViewBinding Resolution;
        [NotNull] public readonly CustomSliderViewBinding VSync;
        [NotNull] public readonly CustomSliderViewBinding FrameRate;
        [NotNull] public readonly CustomSliderViewBinding SimulationRate;
        [NotNull] public readonly ScrollView Control;
        [NotNull] public readonly ScrollView Audio;
        [NotNull] public readonly VisualElement SubmenusContainer;
        [NotNull] public readonly ScrollView Submenus;
        [NotNull] public readonly Button OptionsButton;
        [NotNull] public readonly Button ControlButton;
        [NotNull] public readonly Button AudioButton;
        [NotNull] public readonly Button Back;

        public SettingsViewBinding(VisualElement root)
        {
            Root = root;
            Options = root.Q<ScrollView>("Options") ?? throw new NullReferenceException("\"Options\" not found!");
            Resolution = new CustomSliderViewBinding(root.Q<VisualElement>("Resolution") ?? throw new NullReferenceException("\"Resolution\" not found!"));
            VSync = new CustomSliderViewBinding(root.Q<VisualElement>("VSync") ?? throw new NullReferenceException("\"VSync\" not found!"));
            FrameRate = new CustomSliderViewBinding(root.Q<VisualElement>("FrameRate") ?? throw new NullReferenceException("\"FrameRate\" not found!"));
            SimulationRate = new CustomSliderViewBinding(root.Q<VisualElement>("SimulationRate") ?? throw new NullReferenceException("\"SimulationRate\" not found!"));
            Control = root.Q<ScrollView>("Control") ?? throw new NullReferenceException("\"Control\" not found!");
            Audio = root.Q<ScrollView>("Audio") ?? throw new NullReferenceException("\"Audio\" not found!");
            SubmenusContainer = root.Q<VisualElement>("SubmenusContainer") ?? throw new NullReferenceException("\"SubmenusContainer\" not found!");
            Submenus = root.Q<ScrollView>("Submenus") ?? throw new NullReferenceException("\"Submenus\" not found!");
            OptionsButton = root.Q<Button>("OptionsButton") ?? throw new NullReferenceException("\"OptionsButton\" not found!");
            ControlButton = root.Q<Button>("ControlButton") ?? throw new NullReferenceException("\"ControlButton\" not found!");
            AudioButton = root.Q<Button>("AudioButton") ?? throw new NullReferenceException("\"AudioButton\" not found!");
            Back = root.Q<Button>("Back") ?? throw new NullReferenceException("\"Back\" not found!");
        }
    }
}
