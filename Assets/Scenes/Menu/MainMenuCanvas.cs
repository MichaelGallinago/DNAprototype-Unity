using Scenes.Menu.Model;
using Scenes.Menu.Tube;
using UnityEngine;

namespace Scenes.Menu
{
    public class MainMenuCanvas : MonoBehaviour
    {
        [field: SerializeField] public TubeAnimation TubeAnimation { get; private set; }
        [field: SerializeField] public ModelAnimation ModelAnimation { get; private set; }
    }
}
