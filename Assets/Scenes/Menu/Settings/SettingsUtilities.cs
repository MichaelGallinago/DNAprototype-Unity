using Scenes.Menu.Settings.Audio;
using Scenes.Menu.Settings.Control;
using Scenes.Menu.Settings.Options;
using Scenes.Menu.Settings.Submenus;

namespace Scenes.Menu.Settings
{
    public static class SettingsUtilities
    {
        public static void RegisterCallbacks(MainMenuArgs args)
        {
            SubmenusUtilities.RegisterCallbacks(args);
            OptionsUtilities.Initialize(args);
            ControlUtilities.RegisterCallbacks(args);
            AudioUtilities.RegisterCallbacks(args);
        }
    }
}
