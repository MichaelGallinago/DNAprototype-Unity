using System;
using LitMotion;
using TMPro;
using UnityEngine;

namespace Scenes.Menu.Card
{
    public class CardText : MonoBehaviour
    {
        private static readonly Color BaseColor = Color.white;
        private static readonly Color SelectedColor = new Color32(255, 192, 64, 255);
        
        [SerializeField] private TextMeshProUGUI _text;
        
        private Action<Color> _animation;
        private MotionHandle _handle;
        
        public bool IsSelected
        {
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                
                if (_handle.IsActive())
                {
                    _handle.Complete();
                }
                
                Color from, to;
                if (value)
                {
                    from = BaseColor;
                    to = SelectedColor;
                }
                else
                {
                    from = SelectedColor;
                    to = BaseColor;
                }
                
                _handle = LMotion.Create(from, to, 0.1f).Bind(_animation);
            }
            get => _isSelected;
        }
        private bool _isSelected;
        
        public void Awake()
        {
            _text.color = BaseColor;
            _animation = SetColor;
        }
        
        private void SetColor(Color color) => _text.color = color;
    }
}
