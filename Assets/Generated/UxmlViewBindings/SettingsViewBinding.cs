using System.Diagnostics.CodeAnalysis;
using UnityEngine.UIElements;
using System;
// ReSharper disable CheckNamespace
// ReSharper disable UnusedType.Global
// ReSharper disable NotAccessedField.Global
// ReSharper disable RedundantNullableFlowAttribute

namespace UxmlViewBindings
{
    public readonly struct SettingsViewBinding
    {
        [NotNull] public readonly VisualElement Root;
        [NotNull] public readonly OptionsViewBinding Options;
        [NotNull] public readonly ControlViewBinding Control;
        [NotNull] public readonly AudioViewBinding Audio;
        [NotNull] public readonly SubmenusViewBinding Submenus;

        public SettingsViewBinding(VisualElement root)
        {
            Root = root;
            Options = new OptionsViewBinding(root.Q<VisualElement>("Options") ?? throw new NullReferenceException("\"Options\" not found!"));
            Control = new ControlViewBinding(root.Q<VisualElement>("Control") ?? throw new NullReferenceException("\"Control\" not found!"));
            Audio = new AudioViewBinding(root.Q<VisualElement>("Audio") ?? throw new NullReferenceException("\"Audio\" not found!"));
            Submenus = new SubmenusViewBinding(root.Q<VisualElement>("Submenus") ?? throw new NullReferenceException("\"Submenus\" not found!"));
        }
    }
}
