using UnityEngine;

namespace Scenes.Menu
{
    public class MenuAudioSource : MonoBehaviour
    {
        [SerializeField] private AudioSource _source;
        
        public void Play(AudioClip clip, float volumeScale = 1f) => _source.PlayOneShot(clip, volumeScale);
    }
}
