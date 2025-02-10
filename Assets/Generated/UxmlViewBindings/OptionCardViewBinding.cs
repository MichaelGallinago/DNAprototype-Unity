using System.Diagnostics.CodeAnalysis;
using UnityEngine.UIElements;
using System;

namespace UxmlViewBindings
{
    public readonly struct OptionCardViewBinding
    {
        [NotNull] public readonly VisualElement Root;
        [NotNull] public readonly Button Button;

        public OptionCardViewBinding(VisualElement root)
        {
            Root = root;
            Button = root.Q<Button>("Button") ?? throw new NullReferenceException("\"Button\" not found!");
        }
    }
}
