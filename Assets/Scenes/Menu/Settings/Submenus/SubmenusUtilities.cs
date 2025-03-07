using DnaCore.Settings;
using DnaCore.Utilities;
using Scenes.Menu.OptionCard;
using Scenes.Menu.Settings.Audio;
using Scenes.Menu.Settings.Control;
using Scenes.Menu.Settings.Options;
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
                    static (evt, userArgs) => OnSettingsClosed(evt, userArgs))
                .Register<ClickEvent>(binding.BackButton,
                    static (evt, userArgs) => OnSettingsClosed(evt, userArgs))
                .Register<NavigationSubmitEvent>(binding.BackButton,
                    static (evt, userArgs) => OnSettingsClosed(evt, userArgs))

                .Register<ClickEvent>(binding.OptionsButton,
                    static (evt, userArgs) => OnOptionsClicked(evt, userArgs))
                .Register<NavigationSubmitEvent>(binding.OptionsButton,
                    static (evt, userArgs) => OnOptionsSubmitted(evt, userArgs))

                .Register<ClickEvent>(binding.ControlButton,
                    static (evt, userArgs) => OnControlClicked(evt, userArgs))
                .Register<NavigationSubmitEvent>(binding.ControlButton,
                    static (evt, userArgs) => OnControlSubmitted(evt, userArgs))

                .Register<ClickEvent>(binding.AudioButton,
                    static (evt, userArgs) => OnAudioClicked(evt, userArgs))
                .Register<NavigationSubmitEvent>(binding.AudioButton,
                    static (evt, userArgs) => OnAudioSubmitted(evt, userArgs));
        
        private static void OnSettingsClosed(EventBase e, MainMenuArgs args)
        {
            AppSettings.Save();
            _ = CardsUtilities.Show(args);
            args.Binding.Settings.Root.enabledSelf = false;
            args.Binding.CardSettings.Root.Focus();
        }
        
        private static void OnOptionsClicked(ClickEvent e, MainMenuArgs args)
        {
            HideSubmenus(in args.Binding.Settings);
            OptionsUtilities.Open(args, false);
        }
        
        private static void OnOptionsSubmitted(NavigationSubmitEvent e, MainMenuArgs args)
        {
            HideSubmenus(in args.Binding.Settings);
            OptionsUtilities.Open(args, true);
        }
        
        private static void OnControlClicked(ClickEvent e, MainMenuArgs args)
        {
            HideSubmenus(in args.Binding.Settings);
            ControlUtilities.Open(args, false);
        }
        
        private static void OnControlSubmitted(NavigationSubmitEvent e, MainMenuArgs args)
        {
            HideSubmenus(in args.Binding.Settings);
            ControlUtilities.Open(args, true);
        }
        
        private static void OnAudioClicked(ClickEvent e, MainMenuArgs args)
        {
            HideSubmenus(in args.Binding.Settings);
            AudioUtilities.Open(args, false);
        }
        
        private static void OnAudioSubmitted(NavigationSubmitEvent e, MainMenuArgs args)
        {
            HideSubmenus(in args.Binding.Settings);
            AudioUtilities.Open(args, true);
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
