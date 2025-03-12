using System.Diagnostics.CodeAnalysis;
using UnityEngine.UIElements;
using System;
// ReSharper disable CheckNamespace
// ReSharper disable UnusedType.Global
// ReSharper disable NotAccessedField.Global
// ReSharper disable RedundantNullableFlowAttribute

namespace UxmlViewBindings
{
    public readonly struct ScrollViewViewBinding
    {
        [NotNull] public readonly VisualElement Root;
        [NotNull] public readonly ScrollView ScrollView;

        public ScrollViewViewBinding(VisualElement root)
        {
            Root = root;
            ScrollView = root.Q<ScrollView>("ScrollView") ?? throw new NullReferenceException("\"ScrollView\" not found!");
        }
    }
}
