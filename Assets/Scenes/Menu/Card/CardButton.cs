using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Scenes.Menu.Card
{
    public class CardButton : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] private int _index;
        [SerializeField] private int _menuIndex;
        [SerializeField] private AudioClip _clickSound;
        [SerializeField] private AudioClip _selectSound;
        [SerializeField] private float _soundVolume = 0.05f;
        [SerializeField] private CardText _text;
        [SerializeField] private CardAnimation _animation;
        
        [SerializeField] private UnityEvent<int> _onSelected;
        [SerializeField] private UnityEvent<int> _onPressed;
        [SerializeField] private UnityEvent<AudioClip, float> _onSoundEmitted;
        
        public void EmitPressingWithIndex()
        {
            _onSoundEmitted?.Invoke(_clickSound, _soundVolume);
            _onPressed?.Invoke(_index);
        }
        
        public void Select(int selectedIndex) => _text.IsSelected = selectedIndex == _index;
        
        public void Hide() => _animation.Hide(0f);
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _onSoundEmitted?.Invoke(_selectSound, _soundVolume);
            _onSelected?.Invoke(_index);
        }
    }
}
