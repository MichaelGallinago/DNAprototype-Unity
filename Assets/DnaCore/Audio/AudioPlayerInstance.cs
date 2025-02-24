using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

//TODO: replace "static pitch => SetBgmPitch(pitch)" to method group after C# 11 in unity
namespace DnaCore.Audio
{
    public class AudioPlayerInstance : MonoBehaviour
    {
        [SerializeField] private AudioSource _bgmAudioSource;
        [SerializeField] private AudioSource _sfxAudioSource;

        private MotionHandle _pitchHandle;
        
        public static AudioPlayerInstance Instance { get; private set; }

        public static void Initialize()
        {
            if (Instance) return;
            
            var singletonObject = new GameObject(nameof(AudioPlayerInstance));
            Instance = singletonObject.AddComponent<AudioPlayerInstance>();
            
            Instance._bgmAudioSource = singletonObject.AddComponent<AudioSource>();
            Instance._bgmAudioSource.playOnAwake = false;
            
            Instance._sfxAudioSource = singletonObject.AddComponent<AudioSource>();
            Instance._sfxAudioSource.playOnAwake = false;
            
            DontDestroyOnLoad(singletonObject);
        }

        private void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public MotionHandle SetPitchFadeOut(float duration)
        {
            if (_pitchHandle.IsPlaying())
            {
                _pitchHandle.Cancel();   
            }
            
            return _pitchHandle = LMotion.Create(0f, 1f, duration).WithEase(Ease.OutCubic).BindToPitch(_bgmAudioSource);
        }

        public void PlayBgm(AudioClip clip, float volume)
        {
            _bgmAudioSource.pitch = 1f;
            _bgmAudioSource.volume = volume;
            _bgmAudioSource.clip = clip;
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
