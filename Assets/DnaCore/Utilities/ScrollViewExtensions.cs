using UnityEngine.UIElements;

namespace DnaCore.Utilities
{
    public static class VisualElementExtensions
    {
        public static VisualElement FirstChild(this VisualElement visualElement) => 
            visualElement[0];
        
        public static VisualElement LastChild(this VisualElement visualElement) => 
            visualElement[visualElement.childCount - 1];
    }
}
