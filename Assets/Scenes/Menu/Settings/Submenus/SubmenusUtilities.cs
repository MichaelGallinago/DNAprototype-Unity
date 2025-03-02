using DnaCore.Utilities;
using Scenes.Menu.OptionCard;
using UnityEngine.UIElements;
using UxmlViewBindings;

//TODO: replace "static (evt, userArgs) => ..." to method group after C# 11 in unity
// ReSharper disable UnusedParameter.Local
namespace Scenes.Menu.Settings.Submenus
{
    public static class SubmenusUtilities
    {
        public static void RegisterCallbacks(MainMenuArgs args) => 
            RegisterCallbacks(in args.Binding.Settings.Submenus, args);

        private static void RegisterCallbacks(in SubmenusViewBinding binding, MainMenuArgs args) =>
            new CallbackBuilder<MainMenuArgs>(args)
                .Register<NavigationCancelEvent>(binding.Root,
                    static (evt, userArgs) => OnSettingsCanceled(evt, userArgs))
                .Register<ClickEvent>(binding.BackButton,
                    static (evt, userArgs) => OnSettingsCanceled(evt, userArgs))
                .Register<NavigationSubmitEvent>(binding.BackButton,
                    static (evt, userArgs) => OnSettingsCanceled(evt, userArgs))

                .Register<ClickEvent>(binding.OptionsButton,
                    static (evt, userArgs) => OnOptionsPressed(evt, userArgs))
                .Register<NavigationSubmitEvent>(binding.OptionsButton,
                    static (evt, userArgs) => OnOptionsPressed(evt, userArgs))

                .Register<ClickEvent>(binding.ControlButton,
                    static (evt, userArgs) => OnControlPressed(evt, userArgs))
                .Register<NavigationSubmitEvent>(binding.ControlButton,
                    static (evt, userArgs) => OnControlPressed(evt, userArgs))

                .Register<ClickEvent>(binding.AudioButton,
                    static (evt, userArgs) => OnAudioPressed(evt, userArgs))
                .Register<NavigationSubmitEvent>(binding.AudioButton,
                    static (evt, userArgs) => OnAudioPressed(evt, userArgs));
        
        private static void OnSettingsCanceled(EventBase e, MainMenuArgs args)
        {
            _ = CardsUtilities.Show(args);
            args.Binding.Settings.Root.enabledSelf = false;
        }
        
        private static void OnOptionsPressed(EventBase e, MainMenuArgs args)
        {
            HideSubmenus(in args.Binding.Settings);
            args.Binding.Settings.Options.Root.style.display = DisplayStyle.Flex;
        }
        
        private static void OnControlPressed(EventBase e, MainMenuArgs args)
        {
            HideSubmenus(in args.Binding.Settings);
            args.Binding.Settings.Options.Root.style.display = DisplayStyle.Flex;
        }
        
        private static void OnAudioPressed(EventBase e, MainMenuArgs args)
        {
            HideSubmenus(in args.Binding.Settings);
            args.Binding.Settings.Options.Root.style.display = DisplayStyle.Flex;
        }

        private static void HideSubmenus(in SettingsViewBinding binding)
        {
            binding.Submenus.Root.style.display = DisplayStyle.None;
            binding.Options.Root.style.display = DisplayStyle.None;
            binding.Control.Root.style.display = DisplayStyle.None;
            binding.Audio.Root.style.display = DisplayStyle.None;
        }
    }
}