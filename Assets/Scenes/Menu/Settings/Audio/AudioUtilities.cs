using DnaCore.Utilities;
using Scenes.Menu.Audio;
using UnityEngine.UIElements;
using UxmlViewBindings;

namespace Scenes.Menu.Settings.Audio
{
    //TODO: replace "static (evt, userArgs) => ..." to method group after C# 11 in unity
    // ReSharper disable UnusedParameter.Local
    public static class AudioUtilities
    {
        public static void RegisterCallbacks(MainMenuArgs args) =>
            RegisterCallbacks(in args.Binding.Settings.Audio, args);

        public static void Open(MainMenuArgs args, bool withFocus)
        {
            args.Binding.Settings.Audio.Root.style.display = DisplayStyle.Flex;
            UpdateSliders(in args.Binding.Settings.Audio, args.ViewModel);
            
            if (!withFocus) return;
            args.Binding.Settings.Audio.ScrollView.FirstChild().Focus();
        }
        
        private static void UpdateSliders(in AudioViewBinding binding, MainMenuViewModel viewModel)
        {
            binding.Music.Slider.value = viewModel.BgmVolume;
            binding.Sound.Slider.value = viewModel.SfxVolume;
        }

        private static void RegisterCallbacks(in AudioViewBinding binding, MainMenuArgs args) =>
            new CallbackBuilder<MainMenuArgs>(args)
                .Register<NavigationCancelEvent>(binding.Root,
                    static (evt, userArgs) => OnApplyWithFocus(evt, userArgs))
                .Register<ClickEvent>(binding.Apply.Button,
                    static (evt, userArgs) => OnApply(evt, userArgs))
                .Register<NavigationSubmitEvent>(binding.Apply.Button,
                    static (evt, userArgs) => OnApplyWithFocus(evt, userArgs))
                
                .RegisterValueChanged(binding.Music.Slider, 
                    static (evt, userArgs) => OnMusicVolumeChanged(evt, userArgs))
                .RegisterValueChanged(binding.Sound.Slider, 
                    static (evt, userArgs) => OnSoundVolumeChanged(evt, userArgs));
        
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
