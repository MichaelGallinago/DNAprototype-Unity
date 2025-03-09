using Cysharp.Threading.Tasks;
using DnaCore.Singletons.Audio;
using LitMotion;
using Scenes.Menu.Logo;
using Scenes.Menu.Model;
using Scenes.Menu.OptionCard;
using Scenes.Menu.Tube;

namespace Scenes.Menu
{
    public static class TransitionUtilities
    {
        public static void Enter(MainMenuArgs args)
        {
            args.StartAnimation = LSequence.Create()
                .Join(TubeUtilities.Animate(args))
                .Join(ModelUtilities.Animate(args))
                .Join(LogoUtilities.Show(args))
                .Run();
            
            _ = CardsUtilities.Show(args, 1f);
        }
        
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
            UnityEngine.Application.Quit();
#endif
        }
    }
}
