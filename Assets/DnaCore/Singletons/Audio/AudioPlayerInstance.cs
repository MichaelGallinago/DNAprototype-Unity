using System;
using DnaCore.Settings;
using DnaCore.Utilities;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
using UnityEngine.Audio;

namespace DnaCore.Singletons.Audio
{
    public class AudioPlayerInstance : MonoSingleton<AudioPlayerInstance>
    {
        private const string VolumeName = "MasterVolume";
        
        [SerializeField] private AudioSource _bgmAudioSource;
        [SerializeField] private AudioSource _sfxAudioSource;
        
        [SerializeField] private AudioMixer _bgmAudioMixer;
        [SerializeField] private AudioMixer _sfxAudioMixer;

        private MotionHandle _pitchHandle;

        private void Start()
        {
            SfxVolume = AppSettings.Audio.SfxVolume / 100f;
            BgmVolume = AppSettings.Audio.BgmVolume / 100f;
        }

        public float BgmVolume
        {
            set => _bgmAudioMixer.SetFloat(VolumeName, MathUtilities.FloatToDb(value));
        }
        
        public float SfxVolume
        {
            set => _sfxAudioMixer.SetFloat(VolumeName, MathUtilities.FloatToDb(value));
        }

        public MotionHandle SetPitchFadeOut(float duration)
        {
            if (_pitchHandle.IsPlaying())
            {
                _pitchHandle.Cancel();   
            }
            
            return _pitchHandle = LMotion.Create(0f, 1f, duration).WithEase(Ease.OutCubic).BindToPitch(_bgmAudioSource);
        }

        public void PlayBgm(AudioClip clip, float volume, bool loop = true)
        {
            _bgmAudioSource.pitch = 1f;
            _bgmAudioSource.volume = volume;
            _bgmAudioSource.clip = clip;
            _bgmAudioSource.loop = loop;
            _bgmAudioSource.Play();
        }
        
        public void PlaySfx(AudioClip clip, float volumeScale) => _sfxAudioSource.PlayOneShot(clip, volumeScale);
        public void PlaySfx(AudioClip clip) => _sfxAudioSource.PlayOneShot(clip);

        public MotionHandle StopBgmWithPitchFadeIn(float duration)
        {
            if (_pitchHandle.IsPlaying())
            {
                _pitchHandle.Cancel();   
            }
            
            return _pitchHandle = LMotion.Create(_bgmAudioSource.pitch, 0f, duration)
                .WithEase(Ease.InCubic)
                .WithOnComplete(static () => Instance._bgmAudioSource.Stop())
                .BindToPitch(_bgmAudioSource);
        }
    }
}
