using Cysharp.Text;
using UnityEngine.UIElements;

namespace Scenes.Menu.Settings.CustomElements.Slider
{
    [UxmlElement]
    public partial class LabeledSlider : SliderInt
    {
        [UxmlAttribute] public string Format { get; set; } = string.Empty;

        private readonly Label _valueLabel;
    
        public LabeledSlider()
        {
            Add(_valueLabel = new Label { name = "ValueLabel" });
            
            RegisterCallback<ChangeEvent<int>, LabeledSlider>(
                (e, slider) => slider.UpdateLabel(e.newValue), this);

            RegisterCallback<AttachToPanelEvent, LabeledSlider>(
                (_, slider) => slider.UpdateLabel(slider.value), this);
        }

        private void UpdateLabel(int newValue) => _valueLabel.text = ZString.Format(Format ?? string.Empty, newValue);
    }
}
