using System;
using DnaCore.Utilities;
using LitMotion;
using Scenes.Menu.Audio;
using UnityEngine;

//TODO: replace "static motionArgs => SoundUtilities.PlayLogoSpin(motionArgs)" to method group after C# 11 in unity
namespace Scenes.Menu.Logo
{
    public static class LogoUtilities
    {
        public static MotionHandle Show(MainMenuArgs args) => LSequence.Create()
            .Join(GetRotateMotion(args, 90f, 360f))
            .AppendInterval(0.1f)
            .AppendAction(args, static motionArgs => SoundUtilities.PlayLogoSpin(motionArgs))
            .AppendInterval(0.4f)
            .AppendAction(args, static motionArgs => SoundUtilities.PlayLogoSpin(motionArgs))
            .AppendInterval(0.01f)
            .Run();

        public static MotionHandle Hide(MainMenuArgs args) => LSequence.Create()
            .Join(GetRotateMotion(args, 0f, 90f))
            .AppendInterval(0.2f)
            .AppendAction(args, static motionArgs => SoundUtilities.PlayLogoSpin(motionArgs))
            .AppendInterval(0.01f)
            .Run();
        
        private static MotionHandle GetRotateMotion(MainMenuArgs args, float from, float to) => 
            LMotion.Create(from, to, 1f)
                .WithEase(Ease.InOutQuad)
                .Bind(args, static (value, motionArgs) => SetDegreesToScale(value, motionArgs));

        private static void SetDegreesToScale(float value, MainMenuArgs args)
        {
            Vector3 scale = args.Binding.Logo.transform.scale;
            scale.x = MathF.Cos(value * Mathf.Deg2Rad);
            args.Binding.Logo.transform.scale = scale;
        }
    }
}
