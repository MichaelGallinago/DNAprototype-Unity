using DnaCore.Utilities;
using DnaCore.Utilities.UITK;
using Scenes.Menu.Audio;
using UnityEngine.UIElements;
using UxmlViewBindings;

namespace Scenes.Menu.Settings.Audio
{
    //TODO: replace "static (evt, userArgs) => ..." to method group after C# 11 in unity
    // ReSharper disable UnusedParameter.Local
    public static class AudioUtilities
    {
        public static void Initialize(MainMenuArgs args)
        {
            RegisterCallbacks(in args.Binding.Settings.Audio, args);
            OverrideNavigation(in args.Binding.Settings.Audio, args);
        }

        public static void Open(MainMenuArgs args, bool withFocus)
        {
            args.Binding.Settings.Audio.Root.style.display = DisplayStyle.Flex;
            UpdateSliders(in args.Binding.Settings.Audio, args.ViewModel);
            
            if (!withFocus) return;
            args.Binding.Settings.Audio.ScrollView.FirstChild().FirstChild().Focus();
        }
        
        private static void OverrideNavigation(in AudioViewBinding binding, MainMenuArgs args)
        {
            binding.ScrollView.FirstChild().OverrideNavigation(NavigationMoveEvent.Direction.Up, binding.Apply.Button);
            binding.ScrollView.LastChild().OverrideNavigation(NavigationMoveEvent.Direction.Down, binding.Apply.Button);
            binding.Apply.Button.OverrideNavigation(
                NavigationMoveEvent.Direction.Up, binding.ScrollView.LastChild().FirstChild(), 
                NavigationMoveEvent.Direction.Down, binding.ScrollView.FirstChild().FirstChild());
        }
        
        private static void UpdateSliders(in AudioViewBinding binding, MainMenuViewModel viewModel)
        {
            binding.Music.Slider.SetInitialValue(viewModel.BgmVolume);
            binding.Sound.Slider.SetInitialValue(viewModel.SfxVolume);
        }

        private static void RegisterCallbacks(in AudioViewBinding binding, MainMenuArgs args) =>
            new CallbackBuilder<MainMenuArgs>(args)
                .SetTarget(binding.Root)
                .Register<NavigationCancelEvent>(static (evt, userArgs) => OnApplyWithFocus(evt, userArgs))
                
                .SetTarget(binding.Apply.Button)
                .Register<ClickEvent>(static (evt, userArgs) => OnApply(evt, userArgs))
                .Register<NavigationSubmitEvent>(static (evt, userArgs) => OnApplyWithFocus(evt, userArgs))
                
                .SetValueTarget(binding.Music.Slider)
                .RegisterValueChanged(static (evt, userArgs) => OnMusicVolumeChanged(evt, userArgs))
                
                .SetValueTarget(binding.Sound.Slider)
                .RegisterValueChanged(static (evt, userArgs) => OnSoundVolumeChanged(evt, userArgs));
        
        private static void OnApply(EventBase e, MainMenuArgs args)
        {
            args.Binding.Settings.Audio.Root.style.display = DisplayStyle.None;
            args.Binding.Settings.Submenus.Root.style.display = DisplayStyle.Flex;
        }

        private static void OnMusicVolumeChanged(ChangeEvent<int> e, MainMenuArgs args)
        {
            args.ViewModel.BgmVolume = e.newValue;
            SoundUtilities.PlaySelectWithInterval(e, args);
        }

        private static void OnSoundVolumeChanged(ChangeEvent<int> e, MainMenuArgs args)
        {
            args.ViewModel.SfxVolume = e.newValue;
            SoundUtilities.PlaySelectWithInterval(e, args);
        }
        
        private static void OnApplyWithFocus(EventBase e, MainMenuArgs args)
        {
            OnApply(e, args);
            args.Binding.Settings.Submenus.AudioButton.Focus();
        }
    }
}
