using UnityEngine;

namespace DnaCore.Window
{
    public class WindowControllerInstance : MonoSingleton<WindowControllerInstance>
    {
        public readonly Resolution ReferenceResolution = new() { width = 640, height = 360 };
        
        protected override void Initialize(GameObject singletonObject)
        {
            
        }
        
        private void OnApplicationQuit()
        {
            
        }
    }
}
