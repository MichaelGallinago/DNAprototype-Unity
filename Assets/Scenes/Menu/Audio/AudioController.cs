using LitMotion;
using UnityEngine;

namespace Scenes.Menu.Audio
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private AudioSource _bgmAudioSource;
        [SerializeField] private AudioSource _sfxAudioSource;
        [SerializeField] private float _targetBgmPitch = 1f;
        
        private MotionHandle _pitchHandle;
        
        public MotionHandle SetPitchFadeOut(float duration)
        {
            if (_pitchHandle.IsPlaying())
            {
                _pitchHandle.Cancel();   
            }
            
            return _pitchHandle = LMotion.Create(0f, _targetBgmPitch, duration)
                .WithEase(Ease.OutCubic)
                .Bind(SetBgmPitch);
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
                .WithOnComplete(_bgmAudioSource.Stop)
                .Bind(SetBgmPitch);
        }

        private void SetBgmPitch(float value) => _bgmAudioSource.pitch = value;
    }
}
