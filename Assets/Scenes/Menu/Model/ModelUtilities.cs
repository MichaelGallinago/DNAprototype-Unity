using DnaCore.Audio;
using DnaCore.Utilities;
using LitMotion;

namespace Scenes.Menu.Model
{
    public static class ModelUtilities
    {
        public static MotionHandle Animate(MainMenuArgs args) => LSequence.Create()
            .AppendInterval(3.25f)
            .AppendAndInterval(args.Canvas.ModelAnimation.PlayAppearance(5f), 1.2f)
            .JoinAction(args.AudioStorage.ModelAppearance, 
                static clip => AudioPlayerInstance.Instance.PlaySfx(clip, 0.5f), 1.2f)
            .Run();

        public static MotionHandle Rotate(MainMenuArgs args)
        {
            MotionHandle initialRotation = args.Canvas.ModelAnimation.PlayRotation();
            return LSequence.Create()
                .Append(initialRotation)
                .AppendAction(args.Canvas.ModelAnimation, model => model.LoopRotation())
                .AppendInterval(0.01f)
                .Run();
        }
    }
}