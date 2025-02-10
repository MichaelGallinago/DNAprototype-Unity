using LitMotion;
using UnityEngine;

namespace Scenes.Menu.Audio
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private AudioSource _bgmAudioSource;
        [SerializeField] private AudioSource _sfxAudioSource;
        
        public void PlayBgmWithPitchIncrementation(AudioClip clip, float duration)
        {
            LMotion.Create(0f, _bgmAudioSource.pitch, 10f)
                .WithEase(Ease.OutCubic)
                .Bind(value => { _bgmAudioSource.pitch = value; });
            
            PlayBgm(clip);
        }
        
        public void PlayBgm(AudioClip clip)
        {
            _bgmAudioSource.clip = clip;
            _bgmAudioSource.Play();
        }
        
        public void PlaySfx(AudioClip clip, float volumeScale) => _sfxAudioSource.PlayOneShot(clip, volumeScale);
        public void PlaySfx(AudioClip clip) => _sfxAudioSource.PlayOneShot(clip);
    }
}
