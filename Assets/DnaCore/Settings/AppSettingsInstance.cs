using System;
using System.IO;
using DnaCore.Utilities;
using UnityEngine;

namespace DnaCore.Settings
{
    [Serializable]
    public class AppSettingsInstance
    {
        private const string AppSettingsFileName = "Settings.json";
        
        private static readonly string AppSettingsFilePath = 
            Path.Combine(Application.persistentDataPath, AppSettingsFileName);
        
        public static AppSettingsInstance Instance { get; private set; }

        public Options Options;
        public Audio Audio;
        
        private AppSettingsInstance()
        {
            DisplayInfo currentInfo = Screen.mainWindowDisplayInfo;
            
            var targetFrameRate = (int)currentInfo.refreshRate.value;

            int divisor = MathUtilities.FindGreatestCommonDivisor(currentInfo.width, currentInfo.height);
            var aspectRatio = new AspectRatio(currentInfo.width / divisor, currentInfo.height / divisor);
            
            Options = new Options(
                true,
                aspectRatio,
                aspectRatio.GetMaxScale(currentInfo.width, currentInfo.height),
                default,
                targetFrameRate,
                Math.Min(targetFrameRate, Options.MaxSimulationRate)
            );

            Audio = new Audio(Audio.BaseVolume, Audio.BaseVolume);
        }
        
        public static void Load()
        {
            if (!File.Exists(AppSettingsFilePath))
            {
                Instance = new AppSettingsInstance();
                Save();
                return;
            }

            try
            {
                Instance = JsonUtility.FromJson<AppSettingsInstance>(File.ReadAllText(AppSettingsFilePath));
            }
            catch (ArgumentException e)
            {
                Debug.LogException(e);
                Instance = new AppSettingsInstance();
                Save();
            }
        }

        public static void Save()
        {
            Debug.Log(AppSettingsFilePath);
            File.WriteAllText(AppSettingsFilePath, JsonUtility.ToJson(Instance, true));
        }
    }
}
