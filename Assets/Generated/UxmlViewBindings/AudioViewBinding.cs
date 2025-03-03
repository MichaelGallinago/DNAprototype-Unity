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
        [NotNull] public readonly ApplyViewBinding Apply;

        public AudioViewBinding(VisualElement root)
        {
            Root = root;
            Apply = new ApplyViewBinding(root);
        }
    }
}
