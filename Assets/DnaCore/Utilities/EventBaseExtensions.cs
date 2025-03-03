using UnityEngine.UIElements;

namespace DnaCore.Utilities
{
    public static class EventBaseExtensions
    {
        
        /// <summary>
        /// Replacement for UnityEngine.UIElements.EventBase.PreventDefault()
        /// </summary>
        /// <param name="e"></param>
        /// <param name="focusController"></param>
        public static void PreventFocus(this EventBase e, FocusController focusController)
        {
            e.StopPropagation();
            focusController.IgnoreEvent(e);
        }
    }
}
