using UnityEngine.UIElements;

namespace Scenes.Menu.Settings.CustomElements.Slider
{
    [UxmlElement]
    public partial class VariantsSlider : SliderInt
    {
        [UxmlAttribute] public string[] Variants { get; set; }

        private readonly Label _valueLabel;
    
        public VariantsSlider()
        {
            Add(_valueLabel = new Label { name = "ValueLabel" });
            
            RegisterCallback<ChangeEvent<int>, VariantsSlider>((e, slider) => 
                slider.UpdateLabelVariant(e.newValue), this);

            RegisterCallback<AttachToPanelEvent, VariantsSlider>((_, slider) => 
                slider.UpdateLabelVariant(slider.value), this);
        }

        public void SetInitialValue(int initialValue) => UpdateLabelVariant(value = initialValue);
        
        private void UpdateLabelVariant(int newValue) => _valueLabel.text = Variants is { Length: > 0 } 
            ? Variants[(newValue - lowValue + highValue + 1) % (highValue + 1) % Variants.Length] : string.Empty;
    }
}
