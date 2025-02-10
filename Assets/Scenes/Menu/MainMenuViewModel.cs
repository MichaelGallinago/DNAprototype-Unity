using System;
using Cysharp.Threading.Tasks;
using Scenes.Menu.Audio;
using UnityEngine;

namespace Scenes.Menu
{
    public class MainMenuViewModel : MonoBehaviour
    {
        [SerializeField] private AudioStorage _audioStorage;
        [SerializeField] private AudioController _audioController;
        
        public void PlayCardSound() => _audioController.PlaySfx(_audioStorage.CardMovement, 0.1f);
        public async UniTask PlayMenuTheme()
        {
            _audioController.PlaySfx(_audioStorage.TubeAppearance, 0.5f);
            await UniTask.WaitForSeconds(MathF.Max(_audioStorage.TubeAppearance.length - 1f, 0f));
            _audioController.PlayBgmWithPitchIncrementation(_audioStorage.MenuTheme, 10f);
        }

        public void PlayModelSound()
        {
            _audioController.PlaySfx(_audioStorage.TubeAppearance, 0.5f);
        }
    }
}
