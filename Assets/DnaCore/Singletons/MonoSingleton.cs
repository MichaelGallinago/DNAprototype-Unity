using UnityEngine;

namespace DnaCore.Singletons
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        public static T Instance { get; private set; }
        
        protected virtual void Initialize() {}

        public void RegisterInstance()
        {
            if (Instance == this) return;
            if (Instance || this is not T singleton)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = singleton;
            gameObject.name = typeof(T).Name;
            Initialize();
            DontDestroyOnLoad(gameObject);
        }
    }
}
