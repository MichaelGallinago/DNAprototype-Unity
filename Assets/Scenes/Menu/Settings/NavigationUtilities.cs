using System.Runtime.CompilerServices;
using DnaCore.Utilities;
using UnityEngine.UIElements;

namespace Scenes.Menu.Settings
{
    public static class NavigationUtilities
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OverrideNavigation(
            this VisualElement visualElement, NavigationMoveEvent.Direction direction, VisualElement destination)
        {
            visualElement.RegisterCallback<NavigationMoveEvent, OneWayOverrideArgs>(
                static (e, args) => args.OverrideWay(e), 
                new OneWayOverrideArgs(visualElement, direction, destination));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OverrideNavigation(this VisualElement visualElement, 
            NavigationMoveEvent.Direction directionFirst, VisualElement destinationFirst,
            NavigationMoveEvent.Direction directionSecond, VisualElement destinationSecond)
        {
            visualElement.RegisterCallback<NavigationMoveEvent, TwoWaysOverrideArgs>(
                static (e, args) => args.OverrideWays(e), 
                new TwoWaysOverrideArgs(visualElement, 
                    new Way(directionFirst, destinationFirst), new Way(directionSecond, destinationSecond)));
        }
        
        private readonly struct OneWayOverrideArgs
        {
            private readonly VisualElement _departure;
            private readonly Way _way;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public OneWayOverrideArgs(
                VisualElement departure, NavigationMoveEvent.Direction direction, VisualElement destination)
            {
                _departure = departure;
                _way = new Way(direction, destination);
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void OverrideWay(NavigationMoveEvent e)
            {
                if (e.direction != _way.Direction) return;
                e.PreventFocus(_departure.focusController);
                _way.Destination.Focus();
            }
        }
        
        private readonly struct TwoWaysOverrideArgs
        {
            private readonly VisualElement _departure;
            private readonly Way _way1;
            private readonly Way _way2;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TwoWaysOverrideArgs(VisualElement departure, Way way1, Way way2)
            {
                _departure = departure;
                _way1 = way1;
                _way2 = way2;
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void OverrideWays(NavigationMoveEvent e)
            {
                if (e.direction == _way1.Direction)
                {
                    e.PreventFocus(_departure.focusController);
                    _way1.Destination.Focus();
                    return;
                }
                
                if (e.direction != _way2.Direction) return;
                e.PreventFocus(_departure.focusController);
                _way2.Destination.Focus();
            }
        }

        private readonly struct Way
        {
            public readonly NavigationMoveEvent.Direction Direction;
            public readonly VisualElement Destination;
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Way(NavigationMoveEvent.Direction direction, VisualElement destination)
            {
                Direction = direction;
                Destination = destination;
            }
        }
    }
}
