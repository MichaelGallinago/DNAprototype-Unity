using System.Diagnostics.CodeAnalysis;
using UnityEngine.UIElements;
using System;
// ReSharper disable CheckNamespace
// ReSharper disable UnusedType.Global
// ReSharper disable NotAccessedField.Global
// ReSharper disable RedundantNullableFlowAttribute

namespace UxmlViewBindings
{
    public readonly struct SliderViewBinding
    {
        [NotNull] public readonly VisualElement Root;
        [NotNull] public readonly Scenes.Menu.Settings.CustomElements.Slider.LabeledSlider Slider;

        public SliderViewBinding(VisualElement root)
        {
            Root = root;
            Slider = root.Q<Scenes.Menu.Settings.CustomElements.Slider.LabeledSlider>("Slider") ?? throw new NullReferenceException("\"Slider\" not found!");
        }
    }
}
