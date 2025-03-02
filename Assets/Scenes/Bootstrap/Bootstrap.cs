using DnaCore.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes.Bootstrap
{
    public class Bootstrap : MonoBehaviour
    {
        private void Start() => SceneManager.LoadScene("Scenes/Menu/Menu");
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeSingletons()
        {
            AudioPlayerInstance.Initialize();
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void LoadConfigs()
        {
            //TODO: Load config
        }
    }
}
