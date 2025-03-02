using System.Diagnostics.CodeAnalysis;
using UnityEngine.UIElements;
using System;
// ReSharper disable CheckNamespace
// ReSharper disable UnusedType.Global
// ReSharper disable NotAccessedField.Global
// ReSharper disable RedundantNullableFlowAttribute

namespace UxmlViewBindings
{
    public readonly struct SavesViewBinding
    {
        [NotNull] public readonly VisualElement Root;

        public SavesViewBinding(VisualElement root)
        {
            Root = root;
        }
    }
}
