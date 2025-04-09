using UnityEngine;
using UnityEngine.SceneManagement;

namespace DnaPrototype.Scenes.Bootstrap
{
    public class Bootstrap : MonoBehaviour
    {
        private void Start() => SceneManager.LoadScene("DnaPrototype/Scenes/Menu/Menu");
    }
}
