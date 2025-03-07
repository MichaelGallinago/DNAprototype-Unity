using DnaCore.Audio;
using UnityEngine.UIElements;

namespace Scenes.Menu.Audio
{
    public static class SoundUtilities
    {
        public static void PlayFocus(EventBase e, MainMenuArgs args) => 
            AudioPlayer.PlaySfx(args.AudioStorage.Focus, 0.05f);
        
        public static void PlaySelect(MainMenuArgs args) => 
            AudioPlayer.PlaySfx(args.AudioStorage.Select, 0.1f);
        
        public static void PlayLogoSpin(MainMenuArgs args) =>
            AudioPlayer.PlaySfx(args.AudioStorage.LogoSpin, 0.2f);
        
        public static void PlaySelectWithInterval(EventBase e, MainMenuArgs args)
        {
            if (e.timestamp - args.LastSelectSoundTime > 95f)
            {
                PlaySelect(args);
            }
            args.LastSelectSoundTime = e.timestamp;
        }
    }
}
