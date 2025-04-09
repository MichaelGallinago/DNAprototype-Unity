using DnaCore.Settings;
using DnaCore.Utilities;
using DnaCore.Utilities.LitMotion;
using LitMotion;
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

        public static void Open(MainMenuArgs args, bool withFocus)
        {
            if (!withFocus) return;
            args.Binding.Settings.Submenus.OptionsButton.Focus();
        }
        
        private static void RegisterCallbacks(in SubmenusViewBinding binding, MainMenuArgs args) => 
            new CallbackBuilder<MainMenuArgs>(args)
                .SetTarget(binding.Root)
                .Register<NavigationCancelEvent>(static (evt, userArgs) => OnSettingsClosedFocused(evt, userArgs))
                
                .SetTarget(binding.BackButton)
                .Register<ClickEvent>(static (evt, userArgs) => OnSettingsClosed(evt, userArgs))
                .Register<NavigationSubmitEvent>(static (evt, userArgs) => OnSettingsClosedFocused(evt, userArgs))

                .SetTarget(binding.OptionsButton)
                .Register<ClickEvent>(static (evt, userArgs) => OnOptionsClicked(evt, userArgs))
                .Register<NavigationSubmitEvent>(static (evt, userArgs) => OnOptionsSubmitted(evt, userArgs))

                .SetTarget(binding.ControlButton)
                .Register<ClickEvent>(static (evt, userArgs) => OnControlClicked(evt, userArgs))
                .Register<NavigationSubmitEvent>(static (evt, userArgs) => OnControlSubmitted(evt, userArgs))

                .SetTarget(binding.AudioButton)
                .Register<ClickEvent>(static (evt, userArgs) => OnAudioClicked(evt, userArgs))
                .Register<NavigationSubmitEvent>(static (evt, userArgs) => OnAudioSubmitted(evt, userArgs));
        
        private static void OnSettingsClosedFocused(EventBase e, MainMenuArgs args) =>
            OnSettingsClosed(e, args, true);
        
        private static void OnSettingsClosed(EventBase e, MainMenuArgs args, bool withFocus = false)
        {
            AppSettings.Save();
            args.Binding.Settings.Root.enabledSelf = false;

            if (!withFocus)
            {
                _ = CardsUtilities.Show(args);
                return;
            }
            
            args.IsFocusMuted = true;
            _ = LSequence.Create()
                .Append(CardsUtilities.Show(args))
                .AppendAction(args, motionArgs =>
                {
                    motionArgs.Binding.CardSettings.Button.Focus();
                    motionArgs.IsFocusMuted = false;
                })
                .RunAfterAction();
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
