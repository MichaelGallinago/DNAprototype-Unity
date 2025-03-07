using DnaCore.Utilities;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
using UnityEngine.Audio;

namespace DnaCore.Audio
{
    public class AudioPlayerInstance : MonoSingleton<AudioPlayerInstance>
    {
        private const string VolumeName = "MasterVolume";
        
        [SerializeField] private AudioSource _bgmAudioSource;
        [SerializeField] private AudioSource _sfxAudioSource;
        
        [SerializeField] private AudioMixer _bgmAudioMixer;
        [SerializeField] private AudioMixer _sfxAudioMixer;

        private MotionHandle _pitchHandle;
        
        public float BgmVolume
        {
            set => _bgmAudioMixer.SetFloat(VolumeName, MathUtilities.FloatToDb(value));
        }
        
        public float SfxVolume
        {
            set => _sfxAudioMixer.SetFloat(VolumeName, MathUtilities.FloatToDb(value));
        }

        protected override void Initialize(GameObject singletonObject)
        {
            Instance._bgmAudioSource = GetAudioSource(singletonObject, _bgmAudioMixer);
            Instance._sfxAudioSource = GetAudioSource(singletonObject, _sfxAudioMixer);
        }

        private static AudioSource GetAudioSource(GameObject singletonObject, AudioMixer mixer)
        {
            var bgmSource = singletonObject.AddComponent<AudioSource>();
            bgmSource.playOnAwake = false;
            bgmSource.outputAudioMixerGroup = mixer.outputAudioMixerGroup;
            return bgmSource;
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
