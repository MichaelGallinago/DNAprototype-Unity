using System;
using Cysharp.Text;
using DnaCore.Audio;
using DnaCore.Settings;
using DnaCore.Utilities;
using UnityEngine;

namespace Scenes.Menu
{
    public class MainMenuViewModel : MonoBehaviour
    {
        private AspectRatio _lastAspectRatio;
        private AspectRatio[] _ratios;
        
        public string[] RatioNames => GetRatiosNames(_ratios);

        public string[] ResolutionNames => GetResolutionNames(AppSettings.Options.AspectRatio);

        public bool FullScreen
        {
            set
            {
                AppSettings.Options.FullScreen = value;
                Screen.fullScreenMode = value ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
            }
            get => AppSettings.Options.FullScreen;
        }
        
        public int Ratio
        {
            set => SetRatioWithResolutionValidation(_ratios[value]);
            get => Array.IndexOf(_ratios, AppSettings.Options.AspectRatio);
        }
        
        public int Scale
        {
            set
            {
                AppSettings.Options.Scale = value;
                Vector2Int resolution = AppSettings.Options.AspectRatio.GetScaledResolution(value + 1);
                Screen.SetResolution(resolution.x, resolution.y, Screen.fullScreenMode);
            }
            get => AppSettings.Options.Scale;
        }

        public int VSync
        {
            set
            {
                AppSettings.Options.VSync = value;
                QualitySettings.vSyncCount = value;
            }
            get => AppSettings.Options.VSync;
        }

        public int FrameRate
        {
            set
            {
                Application.targetFrameRate = value;
                AppSettings.Options.TargetFrameRate = value;
            }
            get => AppSettings.Options.TargetFrameRate;
        }

        public int SimulationRate
        {
            set => AppSettings.Options.SimulationRate = value;
            get => AppSettings.Options.SimulationRate;
        }

        public int SfxVolume
        {
            set
            {
                AudioPlayer.SfxVolume = value / 100f;
                AppSettings.Audio.SfxVolume = value;
            }
            get => AppSettings.Audio.SfxVolume;
        }
        
        public int BgmVolume
        {
            set
            {
                AudioPlayer.BgmVolume = value / 100f;
                AppSettings.Audio.BgmVolume = value;
            }
            get => AppSettings.Audio.BgmVolume;
        }
        
        public void UpdateAspectRatios(DisplayInfo currentInfo)
        {
            int divisor = MathUtilities.FindGreatestCommonDivisor(currentInfo.width, currentInfo.height);
            var currentAspectRatio = new AspectRatio(currentInfo.width, currentInfo.height, divisor);
            
            if (_lastAspectRatio.Equals(currentAspectRatio)) return;
            _lastAspectRatio = currentAspectRatio;
            
            int currentIndex = Array.IndexOf(AspectRatio.BuiltIn, currentAspectRatio);
                
            _ratios = new AspectRatio[AspectRatio.BuiltIn.Length + (currentIndex < 0 ? 1 : 0)];
            FillAspectRatios(_ratios, currentIndex, currentAspectRatio);
        }

        private static void SetRatioWithResolutionValidation(AspectRatio ratio)
        {
            AppSettings.Options.AspectRatio = ratio;
                
            DisplayInfo currentInfo = Screen.mainWindowDisplayInfo;
            Vector2Int resolution;
            int scale = AppSettings.Options.Scale + 1;
            do
            {
                resolution = ratio.GetScaledResolution(scale--);
            } while (resolution.x > currentInfo.width || resolution.y > currentInfo.height || scale == 0);
                
            AppSettings.Options.Scale = scale;
            Screen.SetResolution(resolution.x, resolution.y, Screen.fullScreenMode);
        }
        
        private static void FillAspectRatios(AspectRatio[] ratios, int currentIndex, AspectRatio currentAspectRatio)
        {
            if (currentIndex < 0)
            {
                ratios[0] = currentAspectRatio;
                for (var i = 1; i < ratios.Length; i++)
                {
                    ratios[i] = AspectRatio.BuiltIn[i - 1];
                }
            }

            for (var i = 0; i < ratios.Length; i++)
            {
                ratios[i] = AspectRatio.BuiltIn[i];
            }

            if (currentIndex > 0)
            {
                (ratios[0], ratios[currentIndex]) = (ratios[currentIndex], ratios[0]);
            }
        }

        private static string[] GetRatiosNames(AspectRatio[] ratios)
        {
            var aspectRatios = new string[ratios.Length];
            for (var i = 0; i < ratios.Length; i++)
            {
                AspectRatio ratio = ratios[i];
                if (AspectRatio.AspectNameOverrides.TryGetValue(ratio, out string aspectName))
                {
                    aspectRatios[i] = aspectName;
                    continue;
                }
                aspectRatios[i] = ratio.ToString();
            }
            
            return aspectRatios;
        }

        private static string[] GetResolutionNames(AspectRatio ratio)
        {
            int count = GetMaxScale(ratio);
            var aspectRatios = new string[count];
            for (var i = 0; i < count; i++)
            {
                Vector2Int resolution = ratio.GetScaledResolution(i + 1);
                aspectRatios[i] = ZString.Concat(resolution.x, ":", resolution.y);
            }
            return aspectRatios;
        }

        private static int GetMaxScale(AspectRatio ratio)
        {
            DisplayInfo currentInfo = Screen.mainWindowDisplayInfo;
            Vector2Int minResolution = ratio.MinResolution;
            return Math.Min(currentInfo.width / minResolution.x, currentInfo.height / minResolution.y) + 1;
        }
    }
}
