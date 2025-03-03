using DnaCore.Utilities;
using Scenes.Menu.Audio;
using UnityEngine.UIElements;
using UxmlViewBindings;

namespace Scenes.Menu.Settings.Audio
{
    public static class AudioUtilities
    {
        public static void RegisterCallbacks(MainMenuArgs args) =>
            RegisterCallbacks(in args.Binding.Settings.Audio, args);

        public static void Open(MainMenuArgs args, bool withFocus)
        {
            args.Binding.Settings.Audio.Root.style.display = DisplayStyle.Flex;
            
            if (!withFocus) return;
            args.Binding.Settings.Audio.Apply.Button.Focus();
        }

        private static void RegisterCallbacks(in AudioViewBinding binding, MainMenuArgs args) =>
            new CallbackBuilder<MainMenuArgs>(args)
                .Register<NavigationCancelEvent>(binding.Root,
                    static (evt, userArgs) => OnApplyWithFocus(evt, userArgs))
                .Register<ClickEvent>(binding.Apply.Button,
                    static (evt, userArgs) => OnApply(evt, userArgs))
                .Register<NavigationSubmitEvent>(binding.Apply.Button,
                    static (evt, userArgs) => OnApplyWithFocus(evt, userArgs))
            
                .Register<ChangeEvent<int>>(binding.Root, 
                    static (evt, userArgs) => SoundUtilities.PlaySelect(userArgs));
        
        private static void OnApply(EventBase e, MainMenuArgs args)
        {
            args.Binding.Settings.Audio.Root.style.display = DisplayStyle.None;
            args.Binding.Settings.Submenus.Root.style.display = DisplayStyle.Flex;
        }
        
        private static void OnApplyWithFocus(EventBase e, MainMenuArgs args)
        {
            OnApply(e, args);
            args.Binding.Settings.Submenus.AudioButton.Focus();
        }
    }
}
