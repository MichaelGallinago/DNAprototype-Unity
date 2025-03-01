using UxmlViewBindings;

namespace Scenes.Menu.Settings.Audio
{
    public static class AudioUtilities
    {
        public static void RegisterCallbacks(MainMenuArgs args) =>
            RegisterCallbacks(in args.Binding.Settings.Audio, args);

        private static void RegisterCallbacks(in AudioViewBinding binding, MainMenuArgs args)
        {
            
        }
    }
}