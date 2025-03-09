using DnaCore.Settings;
using DnaCore.Singletons.Audio;
using DnaCore.Singletons.Window;
using DnaCore.Utilities;
using UnityEngine;

namespace DnaCore.Singletons
{
    [CreateAssetMenu(
        fileName = nameof(SingletonLoader), 
        menuName = ScriptableObjectUtilities.Folder + nameof(SingletonLoader))]
    public class SingletonLoader : ScriptableObject
    {
        private const int SingletonLoaderIndex = 2;
        
        [SerializeField] private WindowControllerInstance WindowControllerPrefab;
        [SerializeField] private AudioPlayerInstance AudioPlayerPrefab;

        private static SingletonLoader _instance;

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        private static void EditorOnEnable()
        {
            Object asset = UnityEditor.PlayerSettings.GetPreloadedAssets()[SingletonLoaderIndex];
            if (asset is SingletonLoader loader)
            {
                loader.OnEnable();
                return;
            }
            Debug.LogError($"{nameof(SingletonLoader)} Not Found at index {SingletonLoaderIndex} in preloaded assets.");
        }
#endif
        
        private void OnEnable() => _instance = this;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Load()
        {
            AppSettings.Load();
            ApplySettings();
            _instance.InitializePrefabs();
        }

        private static void ApplySettings()
        {
            QualitySettings.vSyncCount = AppSettings.Options.VSync;
            Application.targetFrameRate = AppSettings.Options.TargetFrameRate;
        }
        
        private void InitializePrefabs()
        {
            Instantiate(WindowControllerPrefab).RegisterInstance();
            Instantiate(AudioPlayerPrefab).RegisterInstance();
        }
    }
}
