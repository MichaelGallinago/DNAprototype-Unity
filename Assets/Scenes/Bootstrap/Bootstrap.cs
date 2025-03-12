using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes.Bootstrap
{
    public class Bootstrap : MonoBehaviour
    {
        private void Start() => SceneManager.LoadScene("Scenes/Menu/Menu");
    }
}
