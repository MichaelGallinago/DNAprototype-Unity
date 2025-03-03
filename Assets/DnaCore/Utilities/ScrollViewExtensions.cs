using UnityEngine.UIElements;

namespace DnaCore.Utilities
{
    public static class VisualElementExtensions
    {
        public static VisualElement First(this VisualElement visualElement) => 
            visualElement[0];
        public static VisualElement Last(this VisualElement visualElement) => 
            visualElement[visualElement.childCount - 1];
    }
}
