using LitMotion;
using UnityEngine;

namespace Scenes.Menu.Audio
{
    public class MenuThemeAudioSource : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        
        public void PlayTheme()
        {
            LMotion.Create(0f, _audioSource.pitch, 10f)
                .WithEase(Ease.OutCubic)
                .Bind(value => { _audioSource.pitch = value; });
            
            _audioSource.Play();
        }
    }
}
