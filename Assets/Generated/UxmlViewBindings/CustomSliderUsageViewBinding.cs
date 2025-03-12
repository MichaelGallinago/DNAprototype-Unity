using System.Diagnostics.CodeAnalysis;
using UnityEngine.UIElements;
using System;
// ReSharper disable CheckNamespace
// ReSharper disable UnusedType.Global
// ReSharper disable NotAccessedField.Global
// ReSharper disable RedundantNullableFlowAttribute

namespace UxmlViewBindings
{
    public readonly struct CustomSliderUsageViewBinding
    {
        [NotNull] public readonly VisualElement Root;
        [NotNull] public readonly SliderInt Slider;
        [NotNull] public readonly Label ValueLabel;

        public CustomSliderUsageViewBinding(VisualElement root)
        {
            Root = root;
            Slider = root.Q<SliderInt>("Slider") ?? throw new NullReferenceException("\"Slider\" not found!");
            ValueLabel = root.Q<Label>("ValueLabel") ?? throw new NullReferenceException("\"ValueLabel\" not found!");
        }
    }
}
