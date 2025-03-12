using System.Diagnostics.CodeAnalysis;
using UnityEngine.UIElements;
using System;
// ReSharper disable CheckNamespace
// ReSharper disable UnusedType.Global
// ReSharper disable NotAccessedField.Global
// ReSharper disable RedundantNullableFlowAttribute

namespace UxmlViewBindings
{
    public readonly struct SubmenusViewBinding
    {
        [NotNull] public readonly VisualElement Root;
        [NotNull] public readonly Button OptionsButton;
        [NotNull] public readonly Button ControlButton;
        [NotNull] public readonly Button AudioButton;
        [NotNull] public readonly Button BackButton;

        public SubmenusViewBinding(VisualElement root)
        {
            Root = root;
            OptionsButton = root.Q<Button>("OptionsButton") ?? throw new NullReferenceException("\"OptionsButton\" not found!");
            ControlButton = root.Q<Button>("ControlButton") ?? throw new NullReferenceException("\"ControlButton\" not found!");
            AudioButton = root.Q<Button>("AudioButton") ?? throw new NullReferenceException("\"AudioButton\" not found!");
            BackButton = root.Q<Button>("BackButton") ?? throw new NullReferenceException("\"BackButton\" not found!");
        }
    }
}
