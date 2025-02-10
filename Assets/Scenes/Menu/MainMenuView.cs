using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using UxmlViewBindings;

namespace Scenes.Menu
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        [SerializeField] private MainMenuCanvas _canvas;
        [SerializeField] private MainMenuViewModel _viewModel;
        private MainMenuViewBinding _binding;
        
        private void Awake() => _binding = new MainMenuViewBinding(_document.rootVisualElement);

        private void Start()
        {
            _binding = new MainMenuViewBinding(_document.rootVisualElement);

            _ = AnimateStart();
        }
        
        private async UniTask AnimateStart()
        {
            _ = AnimateTube();
            _ = AnimateModel();
            await UniTask.WaitForSeconds(0.1f);

            SetOptionCardsEnabled(true);
            
            await UniTask.WaitForSeconds(0.1f);
            _viewModel.PlayCardSound();
            await UniTask.WaitForSeconds(0.15f);
            _viewModel.PlayCardSound();
            await UniTask.WaitForSeconds(0.15f);
            _viewModel.PlayCardSound();
        }

        private void SetOptionCardsEnabled(bool isEnabled)
        {
            _binding.OptionSaves.Root.enabledSelf = isEnabled;
            _binding.OptionSettings.Root.enabledSelf = isEnabled;
            _binding.OptionShutdown.Root.enabledSelf = isEnabled;
        }

        private async UniTask AnimateTube()
        {
            _ = _canvas.TubeAnimation.Play();
            await UniTask.WaitForSeconds(0.5f);
            _ = _viewModel.PlayMenuTheme();
        }
        
        private async UniTask AnimateModel()
        {
            _ = _canvas.ModelAnimation.PlayAppearance();
            await UniTask.WaitForSeconds(1.2f);
            _viewModel.PlayModelSound();
        }
    }
}
