using System;
using DnaCore.Settings;
using DnaCore.Singletons.Audio;
using DnaCore.Singletons.Window;
using DnaCore.Utilities;
using DnaCore.Utilities.Mathematics;
using UnityEngine;

namespace Scenes.Menu
{
    public class MainMenuViewModel : MonoBehaviour
    {
        private AspectRatio _lastAspectRatio;
        private AspectRatio[] _ratios;
        
        public string[] GetRatioNames() => AspectRatio.GetRatiosNames(_ratios);

        public string[] GetResolutionNames(DisplayInfo displayInfo) => 
            AppSettings.Options.AspectRatio.GetResolutionNames(displayInfo);
        
        public int Ratio
        {
            get => Array.IndexOf(_ratios, WindowController.Ratio);
            set => WindowController.Ratio = _ratios[value];
        }

        public int Scale
        {
            get => WindowController.Scale;
            set => WindowController.Scale = value;
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
    }
}
