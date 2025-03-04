using System.Diagnostics.CodeAnalysis;
using UnityEngine.UIElements;
using System;
// ReSharper disable CheckNamespace
// ReSharper disable UnusedType.Global
// ReSharper disable NotAccessedField.Global
// ReSharper disable RedundantNullableFlowAttribute

namespace UxmlViewBindings
{
    public readonly struct VariantsSliderViewBinding
    {
        [NotNull] public readonly VisualElement Root;
        [NotNull] public readonly Scenes.Menu.Settings.CustomElements.Slider.VariantsSlider Slider;

        public VariantsSliderViewBinding(VisualElement root)
        {
            Root = root;
            Slider = root.Q<Scenes.Menu.Settings.CustomElements.Slider.VariantsSlider>("Slider") ?? throw new NullReferenceException("\"Slider\" not found!");
        }
    }
}
