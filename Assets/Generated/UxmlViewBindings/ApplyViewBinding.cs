using System.Diagnostics.CodeAnalysis;
using UnityEngine.UIElements;
using System;
// ReSharper disable CheckNamespace
// ReSharper disable UnusedType.Global
// ReSharper disable NotAccessedField.Global
// ReSharper disable RedundantNullableFlowAttribute

namespace UxmlViewBindings
{
    public readonly struct ApplyViewBinding
    {
        [NotNull] public readonly VisualElement Root;
        [NotNull] public readonly Button Button;

        public ApplyViewBinding(VisualElement root)
        {
            Root = root;
            Button = root.Q<Button>("Button") ?? throw new NullReferenceException("\"Button\" not found!");
        }
    }
}
