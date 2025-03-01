using System.Diagnostics.CodeAnalysis;
using UnityEngine.UIElements;
using System;
// ReSharper disable CheckNamespace
// ReSharper disable UnusedType.Global
// ReSharper disable NotAccessedField.Global
// ReSharper disable RedundantNullableFlowAttribute

namespace UxmlViewBindings
{
    public readonly struct ControlViewBinding
    {
        [NotNull] public readonly VisualElement Root;
        [NotNull] public readonly BackButtonViewBinding Apply;

        public ControlViewBinding(VisualElement root)
        {
            Root = root;
            Apply = new BackButtonViewBinding(root.Q<VisualElement>("Apply") ?? throw new NullReferenceException("\"Apply\" not found!"));
        }
    }
}
