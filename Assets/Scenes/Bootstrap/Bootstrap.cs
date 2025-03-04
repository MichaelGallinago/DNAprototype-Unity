using DnaCore.Audio;
using DnaCore.Window;
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
            AudioPlayerInstance.Initialize(nameof(AudioPlayerInstance));
            WindowControllerInstance.Initialize(nameof(WindowControllerInstance));
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void LoadConfigs()
        {
        }
    }
}
