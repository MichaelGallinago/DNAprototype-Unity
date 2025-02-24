using Cysharp.Threading.Tasks;
using DnaCore.Audio;
using LitMotion;
using Scenes.Menu.Logo;

namespace Scenes.Menu
{
    public static class TransitionUtilities
    {
        public static async UniTask Quit(MainMenuArgs args)
        {
            _ = AudioPlayer.StopBgmWithPitchFadeIn(2f);
            _ = args.Canvas.ModelAnimation.PlayDisappearance(1f);
            _ = LogoUtilities.Hide(args);
            await args.Canvas.TubeAnimation.PlayHide(1.5f);
            await UniTask.WaitForSeconds(1f);
            Quit();
        }
        
        private static void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
