using UnityEngine;

namespace DnaCore.Singletons
{
    public abstract class ScriptableSingleton : ScriptableObject
    {
        public abstract void RegisterInstance();
    }
    
    public abstract class ScriptableSingleton<T> : ScriptableSingleton where T : ScriptableSingleton<T>
    {
        public static T Instance { get; private set; }
        
#if UNITY_EDITOR
        private void OnValidate() => OnEnable();

        private void OnEnable()
        {
            SingletonLoader.AddScriptableSingleton(this);
            RegisterInstance();
        }
#endif
        
        private void OnDestroy() => SingletonLoader.RemoveScriptableSingleton(this);

        public sealed override void RegisterInstance()
        {
            if (Instance == this) return;
            if (Instance || this is not T singleton)
            {
                SingletonLoader.RemoveScriptableSingleton(this);
#if UNITY_EDITOR
                UnityEditor.Selection.activeObject = null;
                UnityEditor.AssetDatabase.DeleteAsset(UnityEditor.AssetDatabase.GetAssetPath(this));
#endif
                return;
            }
            
            Instance = singleton;
        }
    }
}
