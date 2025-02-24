using DnaCore.Audio;
using DnaCore.Utilities;
using LitMotion;
using UnityEngine;

namespace Scenes.Menu.Model
{
    public static class ModelUtilities
    {
        public static MotionHandle Animate(MainMenuArgs args) => LSequence.Create()
            .AppendInterval(3.25f)
            .AppendAndInterval(args.Canvas.ModelAnimation.PlayAppearance(5f), 1.2f)
            .AppendAction(args.AudioStorage.ModelAppearance, 
                static clip => AudioPlayerInstance.Instance.PlaySfx(clip, 0.5f))
            .Run();

        public static MotionHandle Rotate(MainMenuArgs args) => LSequence.Create()
            .Append(args.Canvas.ModelAnimation.PlayRotation())
            .AppendAction(args.Canvas.ModelAnimation, model => model.LoopRotation())
            .AppendInterval(0.1f)
            .Run();
    }
}