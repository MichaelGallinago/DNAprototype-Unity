using DnaCore.Audio;
using DnaCore.Utilities;
using LitMotion;
using Scenes.Menu.Model;

namespace Scenes.Menu.Tube
{
    public static class TubeUtilities
    {
        public static MotionHandle Animate(MainMenuArgs args) => LSequence.Create()
            .AppendInterval(0.5f)
            .AppendAndInterval(PlayMenuTheme(args), 0.25f)
            .Append(args.Canvas.TubeAnimation.PlayAppear(10f))
            .Run();
        
        private static MotionHandle PlayMenuTheme(MainMenuArgs args) => LSequence.Create()
            .JoinAction(args.AudioStorage.TubeAppearance, clip => AudioPlayer.PlayBgm(clip, 0.5f))
            .AppendInterval(args.AudioStorage.TubeAppearance.length)
            .AppendAction(args.AudioStorage.MenuTheme, clip => AudioPlayer.PlayBgm(clip, 1f))
            .AppendInterval(0f)
            .JoinAction(() => AudioPlayer.SetPitchFadeOut(10f))
            .AppendInterval(1.1f)
            .Append(ModelUtilities.Rotate(args))
            .Run();
    }
}
