using DnaCore.Singletons.Audio;
using DnaCore.Utilities;
using DnaCore.Utilities.LitMotion;
using LitMotion;

namespace Scenes.Menu.Model
{
    public static class ModelUtilities
    {
        public static MotionHandle Animate(MainMenuArgs args) => LSequence.Create()
            .AppendInterval(3.25f)
            .AppendAndInterval(args.Canvas.ModelAnimation.PlayAppearance(5f), 1.2f)
            .AppendAction(args.AudioStorage.ModelAppearance, 
                static clip => AudioPlayerInstance.Instance.PlaySfx(clip, 0.5f))
            .RunAfterAction();

        public static MotionHandle Rotate(MainMenuArgs args) => LSequence.Create()
            .Append(args.Canvas.ModelAnimation.PlayRotation())
                .AppendAction(args.Canvas.ModelAnimation, model => model.LoopRotation())
                .RunAfterAction();
    }
}
