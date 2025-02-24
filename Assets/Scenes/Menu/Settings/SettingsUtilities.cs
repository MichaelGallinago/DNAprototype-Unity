using Scenes.Menu.OptionCard;
using UnityEngine.UIElements;

//TODO: replace "static (evt, userArgs) => ..." to method group after C# 11 in unity
namespace Scenes.Menu.Settings
{
    public static class SettingsUtilities
    {
        public static void RegisterCallbacks(MainMenuArgs args)
        {
            args.Binding.Settings.SubmenusContainer.RegisterCallback<NavigationCancelEvent, MainMenuArgs>(
                static (evt, userArgs) => OnSettingsCanceled(evt, userArgs), args);
            args.Binding.Settings.Back.RegisterCallback<ClickEvent, MainMenuArgs>(
                static (evt, userArgs) => OnSettingsCanceled(evt, userArgs), args);
            args.Binding.Settings.Back.RegisterCallback<NavigationSubmitEvent, MainMenuArgs>(
                static (evt, userArgs) => OnSettingsCanceled(evt, userArgs), args);

            args.Binding.Settings.OptionsButton.RegisterCallback<ClickEvent, MainMenuArgs>(
                static (evt, userArgs) => OnOptionsPressed(evt, userArgs), args);
            args.Binding.Settings.OptionsButton.RegisterCallback<NavigationSubmitEvent, MainMenuArgs>(
                static (evt, userArgs) => OnOptionsPressed(evt, userArgs), args);
            
            args.Binding.Settings.ControlButton.RegisterCallback<ClickEvent, MainMenuArgs>(
                static (evt, userArgs) => OnControlPressed(evt, userArgs), args);
            args.Binding.Settings.ControlButton.RegisterCallback<NavigationSubmitEvent, MainMenuArgs>(
                static (evt, userArgs) => OnControlPressed(evt, userArgs), args);
            
            args.Binding.Settings.AudioButton.RegisterCallback<ClickEvent, MainMenuArgs>(
                static (evt, userArgs) => OnAudioPressed(evt, userArgs), args);
            args.Binding.Settings.AudioButton.RegisterCallback<NavigationSubmitEvent, MainMenuArgs>(
                static (evt, userArgs) => OnAudioPressed(evt, userArgs), args);
        }
        
        private static void OnOptionsPressed(EventBase e, MainMenuArgs args)
        {
            args.Binding.Settings.SubmenusContainer.visible = false;
        }
        
        private static void OnControlPressed(EventBase e, MainMenuArgs args)
        {
            args.Binding.Settings.SubmenusContainer.enabledSelf = false;
        }
        
        private static void OnAudioPressed(EventBase e, MainMenuArgs args)
        {
            args.Binding.Settings.SubmenusContainer.enabledSelf = false;
        }
        
        private static void OnSettingsCanceled(EventBase e, MainMenuArgs args)
        {
            _ = CardsUtilities.Show(args);
            args.Binding.Root.enabledSelf = false;
        }
    }
}