using System;
using DnaCore.Utilities;
using Scenes.Menu.Audio;
using UnityEngine;
using UnityEngine.UIElements;
using UxmlViewBindings;

//TODO: replace "static (evt, userArgs) => ..." to method group after C# 11 in unity
// ReSharper disable UnusedParameter.Local
namespace Scenes.Menu.Settings.Options
{
    public static class OptionsUtilities
    {
        private static readonly Resolution ReferenceResolution = new() { width = 640, height = 360 };
        
        public static void Initialize(MainMenuArgs args)
        {
            UpdateSlidersLimits(args);
            RegisterCallbacks(in args.Binding.Settings.Options, args);
            OverrideNavigation(in args.Binding.Settings.Options, args);
        }
        
        public static void Open(MainMenuArgs args, bool withFocus)
        {
            args.Binding.Settings.Options.Root.style.display = DisplayStyle.Flex;
            UpdateSlidersLimits(args);
            
            if (!withFocus) return;
            args.Binding.Settings.Options.Resolution.Slider.Focus();
        }
        
        private static void UpdateSlidersLimits(MainMenuArgs args) => 
            SetSlidersLimits(in args.Binding.Settings.Options);
        
        private static void SetSlidersLimits(in OptionsViewBinding binding)
        {
            DisplayInfo currentInfo = Screen.mainWindowDisplayInfo;
            
            binding.Resolution.Slider.highValue = 1 + Math.Min(
                currentInfo.width / ReferenceResolution.width,
                currentInfo.height / ReferenceResolution.height);
            
            binding.FrameRate.Slider.highValue = Math.Max(30, (int)currentInfo.refreshRate.value);
            binding.FrameRate.Slider.lowValue = 30;
            
            binding.SimulationRate.Slider.lowValue = 60;
            binding.SimulationRate.Slider.highValue = 600;
        }

        private static void RegisterCallbacks(in OptionsViewBinding binding, MainMenuArgs args) =>
            new CallbackBuilder<MainMenuArgs>(args)
                .Register<NavigationCancelEvent>(binding.Root,
                    static (evt, userArgs) => OnApplyWithFocus(evt, userArgs))
                .Register<ClickEvent>(binding.Apply.Button,
                    static (evt, userArgs) => OnApply(evt, userArgs))
                .Register<NavigationSubmitEvent>(binding.Apply.Button,
                    static (evt, userArgs) => OnApplyWithFocus(evt, userArgs))
                
                .Register<ChangeEvent<int>>(binding.Root, 
                    static (evt, userArgs) => SoundUtilities.PlaySelect(userArgs))
                
                .Register<ChangeEvent<int>>(binding.Resolution.Slider,
                    static (evt, userArgs) => OnResolutionChanged(evt, userArgs))
                .Register<ChangeEvent<int>>(binding.VSync.Slider,
                    static (evt, userArgs) => OnVSyncChanged(evt, userArgs))
                .Register<ChangeEvent<int>>(binding.FrameRate.Slider,
                    static (evt, userArgs) => OnFrameRateChanged(evt, userArgs))
                .Register<ChangeEvent<int>>(binding.SimulationRate.Slider,
                    static (evt, userArgs) => OnSimulationRateChanged(evt, userArgs));

        private static void OverrideNavigation(in OptionsViewBinding binding, MainMenuArgs args)
        {
            binding.ScrollView.First().OverrideNavigation(NavigationMoveEvent.Direction.Up, binding.Apply.Button);
            binding.ScrollView.Last().OverrideNavigation(NavigationMoveEvent.Direction.Down, binding.Apply.Button);
            binding.Apply.Button.OverrideNavigation(
                NavigationMoveEvent.Direction.Up, binding.ScrollView.Last().First(), 
                NavigationMoveEvent.Direction.Down, binding.ScrollView.First().First());
        }

        private static void OnApply(EventBase e, MainMenuArgs args)
        {
            args.Binding.Settings.Options.Root.style.display = DisplayStyle.None;
            args.Binding.Settings.Submenus.Root.style.display = DisplayStyle.Flex;
        }
        
        private static void OnApplyWithFocus(EventBase e, MainMenuArgs args)
        {
            OnApply(e, args);
            args.Binding.Settings.Submenus.OptionsButton.Focus();
        }
        
        private static void OnResolutionChanged(ChangeEvent<int> e, MainMenuArgs args) =>
            args.ViewModel.Resolution = e.newValue;

        private static void OnVSyncChanged(ChangeEvent<int> e, MainMenuArgs args) => 
            args.ViewModel.VSync = e.newValue;

        private static void OnFrameRateChanged(ChangeEvent<int> e, MainMenuArgs args) =>
            args.ViewModel.FrameRate = e.newValue;
        
        private static void OnSimulationRateChanged(ChangeEvent<int> e, MainMenuArgs args) => 
            args.ViewModel.SimulationRate = e.newValue;
    }
}
