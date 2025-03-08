using DnaCore.Utilities;
using Scenes.Menu.Audio;
using Scenes.Menu.Settings.Audio;
using Scenes.Menu.Settings.Control;
using Scenes.Menu.Settings.Options;
using Scenes.Menu.Settings.Submenus;
using UnityEngine.UIElements;
using UxmlViewBindings;

namespace Scenes.Menu.Settings
{
    public static class SettingsUtilities
    {
        public static void Initialize(MainMenuArgs args)
        {
            RegisterCallbacks(in args.Binding.Settings, args);
            
            SubmenusUtilities.RegisterCallbacks(args);
            OptionsUtilities.Initialize(args);
            ControlUtilities.RegisterCallbacks(args);
            AudioUtilities.Initialize(args);
        }

        public static void Open(MainMenuArgs args, bool withFocus)
        {
            args.Binding.Settings.Root.enabledSelf = true;
            SubmenusUtilities.Open(args, withFocus);
        }

        private static void RegisterCallbacks(in SettingsViewBinding binding, MainMenuArgs args) =>
            new CallbackBuilder<MainMenuArgs>(args)
                .Register<FocusInEvent>(binding.Root, 
                    static (evt, userArgs) => SoundUtilities.PlayFocus(evt, userArgs));

    }
}
