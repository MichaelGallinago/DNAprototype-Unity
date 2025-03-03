using System.Diagnostics.CodeAnalysis;
using UnityEngine.UIElements;
using System;
// ReSharper disable CheckNamespace
// ReSharper disable UnusedType.Global
// ReSharper disable NotAccessedField.Global
// ReSharper disable RedundantNullableFlowAttribute

namespace UxmlViewBindings
{
    public readonly struct AudioViewBinding
    {
        [NotNull] public readonly VisualElement Root;
        [NotNull] public readonly LabeledSliderViewBinding Sound;
        [NotNull] public readonly LabeledSliderViewBinding Music;
        [NotNull] public readonly BackButtonViewBinding Apply;

        public AudioViewBinding(VisualElement root)
        {
            Root = root;
            Sound = new LabeledSliderViewBinding(root.Q<VisualElement>("Sound") ?? throw new NullReferenceException("\"Sound\" not found!"));
            Music = new LabeledSliderViewBinding(root.Q<VisualElement>("Music") ?? throw new NullReferenceException("\"Music\" not found!"));
            Apply = new BackButtonViewBinding(root.Q<VisualElement>("Apply") ?? throw new NullReferenceException("\"Apply\" not found!"));
        }
    }
}
