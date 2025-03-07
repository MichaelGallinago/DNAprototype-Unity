using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace DnaCore.Audio
{
    public class AudioPlayerInstance : MonoSingleton<AudioPlayerInstance>
    {
        [SerializeField] private AudioSource _bgmAudioSource;
        [SerializeField] private AudioSource _sfxAudioSource;

        private MotionHandle _pitchHandle;

        public float BgmVolume
        {
            get => _bgmAudioSource.volume;
            set => _bgmAudioSource.volume = value;
        }
        
        public float SfxVolume
        {
            get => _sfxAudioSource.volume;
            set => _sfxAudioSource.volume = value;
        }

        protected override void Initialize(GameObject singletonObject)
        {
            Instance._bgmAudioSource = singletonObject.AddComponent<AudioSource>();
            Instance._bgmAudioSource.playOnAwake = false;
            
            Instance._sfxAudioSource = singletonObject.AddComponent<AudioSource>();
            Instance._sfxAudioSource.playOnAwake = false;
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
