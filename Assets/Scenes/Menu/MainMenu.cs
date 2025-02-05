using UnityEngine;
using UnityEngine.UIElements;

namespace Scenes.Menu
{
    [RequireComponent(typeof(UIDocument))]
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private VisualTreeAsset _tree;
        
        private void OnValidate() => _tree = GetComponent<UIDocument>().visualTreeAsset;
        
        private void Start()
        {
            var root = new VisualElement();
            _tree.CloneTree(root);
        }
    }
}
