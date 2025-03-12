using System.Runtime.CompilerServices;
using LitMotion;
using UnityEngine;

namespace DnaCore.Singletons.Audio
{
    public static class AudioPlayer
    {
        public static AudioPlayerInstance Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)] get => AudioPlayerInstance.Instance;
        }

        public static float SfxVolume
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)] set => Instance.SfxVolume = value;
        }
        
        public static float BgmVolume
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)] set => Instance.BgmVolume = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MotionHandle SetPitchFadeOut(float duration) => 
            AudioPlayerInstance.Instance.SetPitchFadeOut(duration);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PlayBgm(AudioClip clip, float volume, bool loop = true) =>
            AudioPlayerInstance.Instance.PlayBgm(clip, volume, loop);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PlaySfx(AudioClip clip, float volumeScale) => 
            AudioPlayerInstance.Instance.PlaySfx(clip, volumeScale);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PlaySfx(AudioClip clip) => 
            AudioPlayerInstance.Instance.PlaySfx(clip);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MotionHandle StopBgmWithPitchFadeIn(float duration) => 
            AudioPlayerInstance.Instance.StopBgmWithPitchFadeIn(duration);
    }
}
