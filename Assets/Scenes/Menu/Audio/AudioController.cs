using Cysharp.Threading.Tasks;
using LitMotion;
using UnityEngine;

namespace Scenes.Menu.Audio
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private AudioSource _bgmAudioSource;
        [SerializeField] private AudioSource _sfxAudioSource;

        private float _targetBgmPitch;

        private void Start() => _targetBgmPitch = _bgmAudioSource.pitch;
        
        public void PlayBgmWithPitchFade(AudioClip clip, float volume, float duration)
        {
            StartPitchChanging(0f, _targetBgmPitch, duration, Ease.OutCubic);
            PlayBgm(clip, volume);
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

        public async UniTask StopBgmWithPitchFade(float duration)
        {
            await StartPitchChanging(_bgmAudioSource.pitch, 0f, duration, Ease.InCubic);
            _bgmAudioSource.Stop();
        }
        
        private MotionHandle StartPitchChanging(float from, float to, float duration, Ease ease) =>
            LMotion.Create(from, to, duration)
                .WithEase(ease)
                .Bind(SetBgmPitch);

        private void SetBgmPitch(float value) => _bgmAudioSource.pitch = value;
    }
}
