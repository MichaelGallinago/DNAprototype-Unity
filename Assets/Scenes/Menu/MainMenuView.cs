using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using LitMotion;
using Scenes.Menu.Audio;
using UnityEngine;
using UnityEngine.UIElements;
using UxmlViewBindings;

namespace Scenes.Menu
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private AudioStorage _audioStorage;
        [SerializeField] private AudioController _audioController;
        
        [SerializeField] private UIDocument _document;
        [SerializeField] private MainMenuCanvas _canvas;
        [SerializeField] private MainMenuViewModel _viewModel;
        
        private MainMenuViewBinding _binding;
        private CancellationTokenSource _cts;
        
        private void Start()
        {
            _binding = new MainMenuViewBinding(_document.rootVisualElement);
            RegisterCardsCallbacks();
            
            _cts = new CancellationTokenSource();
            _ = AnimateStart(_cts.Token);
            _ = Cancel();
        }

        private async UniTaskVoid Cancel()
        {
            await UniTask.WaitForSeconds(1f);
            _cts.Cancel();
        }

        private async UniTaskVoid AnimateStart(CancellationToken ct)
        {
            _ = AnimateTube(ct);
            _ = AnimateModel(ct);
            await AnimateLogo();
            _ = ShowCards();
        }

        private async UniTaskVoid ShowCards()
        {
            SetCardsEnabled(true);
            
            await UniTask.WaitForSeconds(0.1f);
            PlayCardSound();
            await UniTask.WaitForSeconds(0.2f);
            PlayCardSound();
            await UniTask.WaitForSeconds(0.2f);
            PlayCardSound();
        }

        private void RegisterCardsCallbacks()
        {
            RegisterCardMouseEnter(_binding.CardSaves);
            RegisterCardMouseEnter(_binding.CardSettings);
            RegisterCardMouseEnter(_binding.CardShutdown);
            
            _binding.CardSaves.Button.RegisterCallback<ClickEvent>(OnSavesPressed);
            _binding.CardSettings.Button.RegisterCallback<ClickEvent>(OnSettingsPressed);
            _binding.CardShutdown.Button.RegisterCallback<ClickEvent>(OnShutdownPressed);
        }
        
        private void OnSavesPressed(ClickEvent e)
        {
            PlayCardSelectSound();
            _ = HideCards();
        }
        
        private void OnSettingsPressed(ClickEvent e)
        {
            PlayCardSelectSound();
            _ = HideCards();
        }

        private void OnShutdownPressed(ClickEvent e)
        {
            PlayCardSelectSound();
            _ = HideCards();
            _ = AnimateQuit();
        }
        
        private async UniTaskVoid AnimateQuit()
        {
            _ = _audioController.StopBgmWithPitchFade(2f);
            _ = _canvas.ModelAnimation.PlayDisappearance(1f);
            _ = HideLogo();
            await _canvas.TubeAnimation.PlayHide(1.5f);
            await UniTask.WaitForSeconds(1f);
            Quit();
        }
        
        private static void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        
        private async UniTaskVoid HideCards()
        {
            SetCardsEnabled(false);
            
            PlayCardSound();
            await UniTask.WaitForSeconds(0.2f);
            PlayCardSound();
            await UniTask.WaitForSeconds(0.2f);
            PlayCardSound();
        }

        private void SetCardsEnabled(bool isEnabled)
        {
            SetCardEnabled(_binding.CardSaves, isEnabled);
            SetCardEnabled(_binding.CardSettings, isEnabled);
            SetCardEnabled(_binding.CardShutdown, isEnabled);
        }
        
        private static void SetCardEnabled(in OptionCardViewBinding card, bool isEnabled) =>
            card.Root.enabledSelf = isEnabled;

        private void RegisterCardMouseEnter(in OptionCardViewBinding card) =>
            card.Button.RegisterCallback<MouseEnterEvent>(PlayHoverSound);

        private void PlayHoverSound(MouseEnterEvent e) => 
            _audioController.PlaySfx(_audioStorage.CardHover, 0.05f);

        private async UniTask AnimateLogo()
        {
            _ = LMotion.Create(90f, 360f, 1f).WithEase(Ease.InOutQuad).Bind(SetDegreesToLogoScale);
            await UniTask.WaitForSeconds(0.1f);
            PlayLogoSpin();
            await UniTask.WaitForSeconds(0.4f);
            PlayLogoSpin();
        }
        
        private async UniTaskVoid HideLogo()
        {
            _ = LMotion.Create(0f, 90f, 1f).WithEase(Ease.InOutQuad).Bind(SetDegreesToLogoScale);
            await UniTask.WaitForSeconds(0.2f);
            PlayLogoSpin();
        }

        private void SetDegreesToLogoScale(float value)
        {
            Vector3 scale = _binding.Logo.transform.scale;
            scale.x = MathF.Cos(value * Mathf.Deg2Rad);
            _binding.Logo.transform.scale = scale;
        }
        
        private void PlayLogoSpin() => _audioController.PlaySfx(_audioStorage.LogoSpin, 0.2f);
        private void PlayCardSound() => _audioController.PlaySfx(_audioStorage.CardMovement, 0.1f);
        private void PlayCardSelectSound() => _audioController.PlaySfx(_audioStorage.CardSelect, 0.1f);
        
        private async UniTaskVoid AnimateTube(CancellationToken ct)
        {
            await UniTask.WaitForSeconds(0.5f, cancellationToken: ct);
            _ = PlayMenuTheme(ct);
            await UniTask.WaitForSeconds(0.25f, cancellationToken: ct);
            await _canvas.TubeAnimation.PlayAppear(10f, ct);
        }
        
        private async UniTaskVoid AnimateModel(CancellationToken ct)
        {
            await UniTask.WaitForSeconds(3.25f, cancellationToken: ct);
            _ = _canvas.ModelAnimation.PlayAppearance(5f, ct);
            await UniTask.WaitForSeconds(1.2f, cancellationToken: ct);
            _audioController.PlaySfx(_audioStorage.ModelAppearance, 0.5f);
        }
        
        private async UniTaskVoid PlayMenuTheme(CancellationToken ct)
        {
            _audioController.PlayBgm(_audioStorage.TubeAppearance, 0.5f, false);
            
            await UniTask.WaitForSeconds(_audioStorage.TubeAppearance.length, cancellationToken: ct);
            _ = _audioController.PlayBgmWithPitchFade(
                _audioStorage.MenuTheme, 1f, 10f, true).ToUniTask(ct);
            
            await UniTask.WaitForSeconds(1.1f, cancellationToken: ct);
            await _canvas.ModelAnimation.PlayRotation();
        }
    }
}
