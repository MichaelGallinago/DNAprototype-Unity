using UnityEngine;

namespace DnaCore
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        public static T Instance { get; private set; }
        
        public static void Initialize(string name)
        {
            if (Instance) return;
            
            var singletonObject = new GameObject(name);
            Instance = singletonObject.AddComponent<T>();
            
            Instance.Initialize(singletonObject);
            
            DontDestroyOnLoad(singletonObject);
        }

        protected virtual void Initialize(GameObject singletonObject) {}
        
        private void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
    }
}
