using DnaCore.Audio;
using DnaCore.Settings;
using DnaCore.Window;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes.Bootstrap
{
    public class Bootstrap : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void LoadConfigs()
        {
            AppSettings.Load();
            Debug.Log("1");
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeSingletons()
        {
            AudioPlayer.Initialize(nameof(AudioPlayer));
            WindowController.Initialize(nameof(WindowController));
            
            Debug.Log("2");
            ApplySettings();
        }

        private static void ApplySettings()
        {
            AudioPlayer.SfxVolume = AppSettings.Audio.SfxVolume;
            AudioPlayer.BgmVolume = AppSettings.Audio.BgmVolume;

            FullScreenMode fullScreen = 
                AppSettings.Options.FullScreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
            Vector2Int resolution = 
                AppSettings.Options.AspectRatio.GetScaledResolution(AppSettings.Options.Scale + 1);
            Screen.SetResolution(resolution.x, resolution.y, Screen.fullScreenMode = fullScreen);
            
            QualitySettings.vSyncCount = AppSettings.Options.VSync;
            Application.targetFrameRate = AppSettings.Options.TargetFrameRate;
        }
        
        private void Start() => SceneManager.LoadScene("Scenes/Menu/Menu");
    }
}
