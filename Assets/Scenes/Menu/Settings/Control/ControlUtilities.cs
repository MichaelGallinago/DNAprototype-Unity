using DnaCore.Utilities;
using Scenes.Menu.Audio;
using UnityEngine.UIElements;
using UxmlViewBindings;

namespace Scenes.Menu.Settings.Control
{
    public static class ControlUtilities
    {
        public static void RegisterCallbacks(MainMenuArgs args) =>
            RegisterCallbacks(in args.Binding.Settings.Control, args);

        public static void Open(MainMenuArgs args, bool withFocus)
        {
            args.Binding.Settings.Control.Root.style.display = DisplayStyle.Flex;
            
            if (!withFocus) return;
            args.Binding.Settings.Control.Apply.Button.Focus();
        }

        private static void RegisterCallbacks(in ControlViewBinding binding, MainMenuArgs args) =>
            new CallbackBuilder<MainMenuArgs>(args)
                .Register<NavigationCancelEvent>(binding.Root,
                    static (evt, userArgs) => OnApplyWithFocus(evt, userArgs))
                .Register<ClickEvent>(binding.Apply.Button,
                    static (evt, userArgs) => OnApply(evt, userArgs))
                .Register<NavigationSubmitEvent>(binding.Apply.Button,
                    static (evt, userArgs) => OnApplyWithFocus(evt, userArgs));
        
        private static void OnApply(EventBase e, MainMenuArgs args)
        {
            args.Binding.Settings.Control.Root.style.display = DisplayStyle.None;
            args.Binding.Settings.Submenus.Root.style.display = DisplayStyle.Flex;
        }
        
        private static void OnApplyWithFocus(EventBase e, MainMenuArgs args)
        {
            OnApply(e, args);
            args.Binding.Settings.Submenus.ControlButton.Focus();
        }
    }
}
