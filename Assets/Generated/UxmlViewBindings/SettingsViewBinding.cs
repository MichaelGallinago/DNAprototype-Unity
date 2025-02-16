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
        [NotNull] public readonly SliderInt Resolution;
        [NotNull] public readonly Label ResolutionLabel;
        [NotNull] public readonly SliderInt VSync;
        [NotNull] public readonly SliderInt FrameRate;
        [NotNull] public readonly SliderInt SimulationRate;

        public SettingsViewBinding(VisualElement root)
        {
            Root = root;
            Control = root.Q<ScrollView>("Control") ?? throw new NullReferenceException("\"Control\" not found!");
            Resolution = root.Q<SliderInt>("Resolution") ?? throw new NullReferenceException("\"Resolution\" not found!");
            ResolutionLabel = root.Q<Label>("ResolutionLabel") ?? throw new NullReferenceException("\"ResolutionLabel\" not found!");
            VSync = root.Q<SliderInt>("VSync") ?? throw new NullReferenceException("\"VSync\" not found!");
            FrameRate = root.Q<SliderInt>("FrameRate") ?? throw new NullReferenceException("\"FrameRate\" not found!");
            SimulationRate = root.Q<SliderInt>("SimulationRate") ?? throw new NullReferenceException("\"SimulationRate\" not found!");
        }
    }
}
