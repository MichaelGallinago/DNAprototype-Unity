using System;
using DnaCore.Utilities;
using Scenes.Menu.Audio;
using Scenes.Menu.Settings.CustomElements.Slider;
using UnityEngine;
using UnityEngine.UIElements;
using UxmlViewBindings;

//TODO: replace "static (evt, userArgs) => ..." to method group after C# 11 in unity
// ReSharper disable UnusedParameter.Local
namespace Scenes.Menu.Settings.Options
{
    public static class OptionsUtilities
    {
        public static void Initialize(MainMenuArgs args)
        {
            UpdateSliders(in args.Binding.Settings.Options, args.ViewModel);
            RegisterCallbacks(in args.Binding.Settings.Options, args);
            OverrideNavigation(in args.Binding.Settings.Options, args);
        }
        
        public static void Open(MainMenuArgs args, bool withFocus)
        {
            args.Binding.Settings.Options.Root.style.display = DisplayStyle.Flex;
            UpdateSliders(in args.Binding.Settings.Options, args.ViewModel);
            
            if (!withFocus) return;
            args.Binding.Settings.Options.ScrollView.FirstChild().FirstChild().Focus();
        }
        
        private static void UpdateSliders(in OptionsViewBinding binding, MainMenuViewModel viewModel)
        {
            DisplayInfo currentInfo = Screen.mainWindowDisplayInfo;
            
            viewModel.UpdateAspectRatios(currentInfo);
            
            SetSliderVariants(binding.AspectRatio.Slider, viewModel.RatioNames);
            SetSliderVariants(binding.Resolution.Slider, viewModel.ResolutionNames);

            binding.FrameRate.Slider.highValue = 
                Math.Max(binding.FrameRate.Slider.lowValue, (int)currentInfo.refreshRate.value);

            binding.SimulationRate.Slider.highValue = DnaCore.Settings.Options.MaxSimulationRate;
            
            //TODO: fullscreen
            binding.AspectRatio.Slider.SetInitialValue(viewModel.Ratio);
            binding.Resolution.Slider.SetInitialValue(viewModel.Scale);
            binding.VSync.Slider.SetInitialValue(viewModel.VSync);
            binding.FrameRate.Slider.SetInitialValue(viewModel.FrameRate);
            binding.SimulationRate.Slider.SetInitialValue(viewModel.SimulationRate);
        }

        private static void SetSliderVariants(VariantsSlider slider, string[] variants)
        {
            slider.Variants = variants;
            slider.highValue = variants.Length - 1;
        }

        private static void RegisterCallbacks(in OptionsViewBinding binding, MainMenuArgs args) =>
            new CallbackBuilder<MainMenuArgs>(args)
                .SetTarget(binding.Root)
                .Register<NavigationCancelEvent>(static (evt, userArgs) => OnApplyWithFocus(evt, userArgs))
                
                .SetTarget(binding.Apply.Button)
                .Register<ClickEvent>(static (evt, userArgs) => OnApply(evt, userArgs))
                .Register<NavigationSubmitEvent>(static (evt, userArgs) => OnApplyWithFocus(evt, userArgs))
                
                .SetTarget(binding.ScrollView)
                .Register<ChangeEvent<int>>(static (evt, userArgs) => SoundUtilities.PlaySelectWithInterval(evt, userArgs))
                
                .SetValueTarget(binding.AspectRatio.Slider)
                .RegisterValueChanged(static (evt, userArgs) => OnAspectRatioChanged(evt, userArgs))
                
                .SetValueTarget(binding.Resolution.Slider)
                .RegisterValueChanged(static (evt, userArgs) => OnResolutionChanged(evt, userArgs))
                
                .SetValueTarget(binding.VSync.Slider)
                .RegisterValueChanged(static (evt, userArgs) => OnVSyncChanged(evt, userArgs))
                
                .SetValueTarget(binding.FrameRate.Slider)
                .RegisterValueChanged(static (evt, userArgs) => OnFrameRateChanged(evt, userArgs))
                
                .SetValueTarget(binding.SimulationRate.Slider)
                .RegisterValueChanged(static (evt, userArgs) => OnSimulationRateChanged(evt, userArgs));

        private static void OverrideNavigation(in OptionsViewBinding binding, MainMenuArgs args)
        {
            binding.ScrollView.FirstChild().OverrideNavigation(NavigationMoveEvent.Direction.Up, binding.Apply.Button);
            binding.ScrollView.LastChild().OverrideNavigation(NavigationMoveEvent.Direction.Down, binding.Apply.Button);
            binding.Apply.Button.OverrideNavigation(
                NavigationMoveEvent.Direction.Up, binding.ScrollView.LastChild().FirstChild(), 
                NavigationMoveEvent.Direction.Down, binding.ScrollView.FirstChild().FirstChild());
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
        
        //TODO: use or remove
        private static void OnFullScreenChanged(ChangeEvent<int> e, MainMenuArgs args) =>
            args.ViewModel.FullScreen = e.newValue > 0;
        
        private static void OnAspectRatioChanged(ChangeEvent<int> e, MainMenuArgs args) =>
            args.ViewModel.Ratio = e.newValue;
        
        private static void OnResolutionChanged(ChangeEvent<int> e, MainMenuArgs args) =>
            args.ViewModel.Scale = e.newValue;

        private static void OnVSyncChanged(ChangeEvent<int> e, MainMenuArgs args) => 
            args.ViewModel.VSync = e.newValue;

        private static void OnFrameRateChanged(ChangeEvent<int> e, MainMenuArgs args) =>
            args.ViewModel.FrameRate = e.newValue;
        
        private static void OnSimulationRateChanged(ChangeEvent<int> e, MainMenuArgs args) => 
            args.ViewModel.SimulationRate = e.newValue;
    }
}
