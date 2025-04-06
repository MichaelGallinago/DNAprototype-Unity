using System.Collections.Generic;
using DnaCore.Settings;
using DnaCore.Utilities;
using UnityEngine;

namespace DnaCore.Singletons
{
    [CreateAssetMenu(
        fileName = nameof(SingletonLoader), 
        menuName = AssetMenuPaths.ScriptableObjects + nameof(SingletonLoader))]
    public class SingletonLoader : ScriptableObject
    {
        private const int SingletonLoaderIndex = 2;
        
        [SerializeField] private List<MonoSingleton> _monoSingletons;
        [SerializeField] private List<ScriptableSingleton> _scriptableSingletons;
        
        private static SingletonLoader _instance;

#if UNITY_EDITOR
        public static void AddScriptableSingleton(ScriptableSingleton scriptableSingleton)
        {
            _instance._scriptableSingletons ??= new List<ScriptableSingleton>();
            if (_instance._scriptableSingletons.Contains(scriptableSingleton)) return;
            _instance._scriptableSingletons.Add(scriptableSingleton);
        }

        public static void RemoveScriptableSingleton(ScriptableSingleton scriptableSingleton) =>
            _instance._scriptableSingletons?.Remove(scriptableSingleton);

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

        private void OnValidate()
        {
            _monoSingletons.RemoveAll(singleton => singleton == null);
            _scriptableSingletons.RemoveAll(singleton => singleton == null);
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
            foreach (MonoSingleton monoSingleton in _monoSingletons) 
            {
                monoSingleton.RegisterInstance();
            }
            
            foreach (ScriptableSingleton scriptableSingleton in _scriptableSingletons)
            {
                scriptableSingleton.RegisterInstance();
            }
        }
    }
}
