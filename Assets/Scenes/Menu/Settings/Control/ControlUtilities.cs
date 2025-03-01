using UxmlViewBindings;

namespace Scenes.Menu.Settings.Control
{
    public static class ControlUtilities
    {
        public static void RegisterCallbacks(MainMenuArgs args) =>
            RegisterCallbacks(in args.Binding.Settings.Control, args);

        private static void RegisterCallbacks(in ControlViewBinding binding, MainMenuArgs args)
        {
            
        }
    }
}