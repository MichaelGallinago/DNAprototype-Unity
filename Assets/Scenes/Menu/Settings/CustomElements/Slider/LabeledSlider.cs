using Cysharp.Text;
using UnityEngine.UIElements;

namespace Scenes.Menu.Settings.CustomElements.Slider
{
    [UxmlElement]
    public partial class LabeledSlider : SliderInt
    {
        public static readonly BindingId FormatProperty = (BindingId)nameof(Format);

        [UxmlAttribute]
        public string Format
        {
            get => _format;
            set
            {
                if (_format == value) return;
                _format = value;
                NotifyPropertyChanged(in FormatProperty);
            }
        }
        private string _format = string.Empty;
        
        private readonly Label _valueLabel;
    
        public LabeledSlider()
        {
            Add(_valueLabel = new Label());
        
            RegisterCallback<ChangeEvent<int>, LabeledSlider>((e, slider) => 
                    slider._valueLabel.text = ZString.Format(slider.Format ?? string.Empty, e.newValue), this);
        }
    }
}
