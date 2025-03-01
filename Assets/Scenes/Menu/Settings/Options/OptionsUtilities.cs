using System;
using UnityEngine;
using UnityEngine.UIElements;
using UxmlViewBindings;

namespace Scenes.Menu.Settings.Options
{
    public static class OptionsUtilities
    {
        private static readonly Resolution ReferenceResolution = new() { width = 640, height = 360 };
        private static Resolution MaxResolution => Screen.resolutions[^1];
        
        public static void Initialize(MainMenuArgs args)
        {
            SetSlidersLimits(args);
            RegisterCallbacks(in args.Binding.Settings.Options, args);
        }
        
        public static void SetSlidersLimits(MainMenuArgs args) => 
            SetSlidersLimits(in args.Binding.Settings.Options, args);
        
        private static void SetSlidersLimits(in OptionsViewBinding binding, MainMenuArgs args)
        {
            binding.Resolution.Slider.highValue = 1 + Math.Min(
                MaxResolution.width / ReferenceResolution.width,
                MaxResolution.height / ReferenceResolution.height);
            
            binding.FrameRate.Slider.highValue = Math.Min(30, (int)Screen.mainWindowDisplayInfo.refreshRate.value);
            binding.FrameRate.Slider.lowValue = 30;
            
            binding.SimulationRate.Slider.lowValue = 60;
            binding.SimulationRate.Slider.highValue = 600;
        }
        
        private static void RegisterCallbacks(in OptionsViewBinding binding, MainMenuArgs args)
        {
            binding.Root.RegisterCallback<NavigationCancelEvent, MainMenuArgs>(
                static (evt, userArgs) => OnApply(evt, userArgs), args);
            binding.Apply.Button.RegisterCallback<ClickEvent, MainMenuArgs>(
                static (evt, userArgs) => OnApply(evt, userArgs), args);
            binding.Apply.Button.RegisterCallback<NavigationSubmitEvent, MainMenuArgs>(
                static (evt, userArgs) => OnApply(evt, userArgs), args);
            
            binding.Resolution.Slider.RegisterCallback<ChangeEvent<int>, MainMenuArgs>(
                static (evt, userArgs) => OnResolutionChanged(evt, userArgs), args);
            binding.VSync.Slider.RegisterCallback<ChangeEvent<int>, MainMenuArgs>(
                static (evt, userArgs) => OnVSyncChanged(evt, userArgs), args);
            binding.FrameRate.Slider.RegisterCallback<ChangeEvent<int>, MainMenuArgs>(
                static (evt, userArgs) => OnFrameRateChanged(evt, userArgs), args);
            binding.SimulationRate.Slider.RegisterCallback<ChangeEvent<int>, MainMenuArgs>(
                static (evt, userArgs) => OnSimulationRateChanged(evt, userArgs), args);
        }

        private static void OnApply(EventBase e, MainMenuArgs args)
        {
            
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
