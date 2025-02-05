using UnityEngine;
using UnityEngine.Events;

namespace Scenes.Menu
{
    public class MenuCanvas : MonoBehaviour
    {
        [SerializeField] private UnityEvent<int> _onCardSelected;
        [SerializeField] private UnityEvent<int> _onMenuChanges;
        
        private int _index;
        private int _menuIndex;
        
        private void Start() => _onCardSelected?.Invoke(_index);
        
        public void SelectCard(int index)
        {
            _index = index;
            _onCardSelected?.Invoke(index);
        }
        
        public void ChangeMenu(int index)
        {
            _index = index;
            _onCardSelected?.Invoke(index);
        }
    }
}
