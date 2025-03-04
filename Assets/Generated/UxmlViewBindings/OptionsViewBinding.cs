using System.Diagnostics.CodeAnalysis;
using UnityEngine.UIElements;
using System;
// ReSharper disable CheckNamespace
// ReSharper disable UnusedType.Global
// ReSharper disable NotAccessedField.Global
// ReSharper disable RedundantNullableFlowAttribute

namespace UxmlViewBindings
{
    public readonly struct OptionsViewBinding
    {
        [NotNull] public readonly VisualElement Root;
        [NotNull] public readonly ScrollView ScrollView;
        [NotNull] public readonly VariantsSliderViewBinding AspectRatio;
        [NotNull] public readonly VariantsSliderViewBinding Resolution;
        [NotNull] public readonly VariantsSliderViewBinding VSync;
        [NotNull] public readonly LabeledSliderViewBinding FrameRate;
        [NotNull] public readonly LabeledSliderViewBinding SimulationRate;
        [NotNull] public readonly BackButtonViewBinding Apply;

        public OptionsViewBinding(VisualElement root)
        {
            Root = root;
            ScrollView = root.Q<ScrollView>("ScrollView") ?? throw new NullReferenceException("\"ScrollView\" not found!");
            AspectRatio = new VariantsSliderViewBinding(root.Q<VisualElement>("AspectRatio") ?? throw new NullReferenceException("\"AspectRatio\" not found!"));
            Resolution = new VariantsSliderViewBinding(root.Q<VisualElement>("Resolution") ?? throw new NullReferenceException("\"Resolution\" not found!"));
            VSync = new VariantsSliderViewBinding(root.Q<VisualElement>("VSync") ?? throw new NullReferenceException("\"VSync\" not found!"));
            FrameRate = new LabeledSliderViewBinding(root.Q<VisualElement>("FrameRate") ?? throw new NullReferenceException("\"FrameRate\" not found!"));
            SimulationRate = new LabeledSliderViewBinding(root.Q<VisualElement>("SimulationRate") ?? throw new NullReferenceException("\"SimulationRate\" not found!"));
            Apply = new BackButtonViewBinding(root.Q<VisualElement>("Apply") ?? throw new NullReferenceException("\"Apply\" not found!"));
        }
    }
}
