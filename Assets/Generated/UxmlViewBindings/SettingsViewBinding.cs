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
        [NotNull] public readonly ScrollView Control;
        [NotNull] public readonly CustomSliderViewBinding Resolution;
        [NotNull] public readonly CustomSliderViewBinding VSync;
        [NotNull] public readonly CustomSliderViewBinding FrameRate;
        [NotNull] public readonly CustomSliderViewBinding SimulationRate;

        public SettingsViewBinding(VisualElement root)
        {
            Root = root;
            Control = root.Q<ScrollView>("Control") ?? throw new NullReferenceException("\"Control\" not found!");
            Resolution = new CustomSliderViewBinding(root.Q<VisualElement>("Resolution") ?? throw new NullReferenceException("\"Resolution\" not found!"));
            VSync = new CustomSliderViewBinding(root.Q<VisualElement>("VSync") ?? throw new NullReferenceException("\"VSync\" not found!"));
            FrameRate = new CustomSliderViewBinding(root.Q<VisualElement>("FrameRate") ?? throw new NullReferenceException("\"FrameRate\" not found!"));
            SimulationRate = new CustomSliderViewBinding(root.Q<VisualElement>("SimulationRate") ?? throw new NullReferenceException("\"SimulationRate\" not found!"));
        }
    }
}
