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
        
        [SerializeField] private List<MonoSingleton> _monoSingletonPrefabs;
        [SerializeField] private List<ScriptableSingleton> _scriptableSingletons;
        
        private static SingletonLoader _instance;
        
        private void OnEnable()
        {
            _instance = this;
#if UNITY_EDITOR
            _monoSingletonPrefabs.RemoveAll(singleton => singleton == null);
            _scriptableSingletons.RemoveAll(singleton => singleton == null);
#endif
        }

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
#endif
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Load()
        {
            AppSettings.Load();
            AppSettings.Apply();
            _instance.InitializePrefabs();
        }
        
        private void InitializePrefabs()
        {
            foreach (MonoSingleton monoSingleton in _monoSingletonPrefabs) 
            {
                Instantiate(monoSingleton).RegisterInstance();
            }
            
            foreach (ScriptableSingleton scriptableSingleton in _scriptableSingletons)
            {
                scriptableSingleton.RegisterInstance();
            }
        }
    }
}
