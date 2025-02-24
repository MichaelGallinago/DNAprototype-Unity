using DnaCore.Utilities;
using UnityEngine;

namespace Scenes.Menu.Audio
{
    [CreateAssetMenu(
        fileName = nameof(AudioStorage), 
        menuName = ScriptableObjectUtilities.Folder + nameof(AudioStorage))]
    public class AudioStorage : ScriptableObject
    {
        [field: SerializeField] public AudioClip TubeAppearance { get; private set; }
        [field: SerializeField] public AudioClip MenuTheme { get; private set; }
        [field: SerializeField] public AudioClip CardMovement { get; private set; }
        [field: SerializeField] public AudioClip ModelAppearance { get; private set; }
        [field: SerializeField] public AudioClip LogoSpin { get; private set; }
        [field: SerializeField] public AudioClip Focus { get; private set; }
        [field: SerializeField] public AudioClip Select { get; private set; }
    }
}
