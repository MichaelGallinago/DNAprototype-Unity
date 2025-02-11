using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using LitMotion;
using Scenes.Menu.Audio;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities;
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
        private MotionHandle _startAnimation;
        
        private void Start()
        {
            _binding = new MainMenuViewBinding(_document.rootVisualElement);
            RegisterCardsCallbacks();
            
            AnimateStart();
        }
        
        private void AnimateStart()
        {
            _startAnimation = LSequence.Create().Join(AnimateTube()).Join(AnimateModel()).Join(AnimateLogo()).Run();
            _ = ShowCards();
        }

        private async UniTask ShowCards()
        {
            SetCardEnabled(_binding.CardSaves, true);
            SetCardEnabled(_binding.CardSettings, true);
            SetCardEnabled(_binding.CardShutdown, true);
            
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
            _startAnimation.Cancel();
            PlayCardSelectSound();
            _ = HideCards();
            _ = AnimateQuit();
        }
        
        private async UniTask AnimateQuit()
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
        
        private async UniTask HideCards()
        {
            SetCardEnabled(_binding.CardSaves, false);
            SetCardEnabled(_binding.CardSettings, false);
            SetCardEnabled(_binding.CardShutdown, false);
            
            PlayCardSound();
            await UniTask.WaitForSeconds(0.2f);
            PlayCardSound();
            await UniTask.WaitForSeconds(0.2f);
            PlayCardSound();
        }
        
        private static void SetCardEnabled(in OptionCardViewBinding card, bool isEnabled)
        {
            card.Root.enabledSelf = isEnabled;
            card.Button.enabledSelf = isEnabled;
        }

        private void RegisterCardMouseEnter(in OptionCardViewBinding card) =>
            card.Button.RegisterCallback<MouseEnterEvent>(PlayHoverSound);

        private void PlayHoverSound(MouseEnterEvent e) => 
            _audioController.PlaySfx(_audioStorage.CardHover, 0.05f);

        private MotionHandle AnimateLogo() => LSequence.Create()
            .Join(LMotion.Create(90f, 360f, 1f).WithEase(Ease.InOutQuad).Bind(SetDegreesToLogoScale))
            .AppendInterval(0.1f)
            .JoinAction(PlayLogoSpin)
            .AppendInterval(0.4f)
            .JoinAction(PlayLogoSpin)
            .Run();
        
        private async UniTask HideLogo()
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

        private MotionHandle AnimateTube() => LSequence.Create()
            .AppendInterval(0.5f)
            .Join(PlayMenuTheme())
            .AppendInterval(0.25f)
            .Append(_canvas.TubeAnimation.PlayAppear(10f))
            .Run();

        private MotionHandle AnimateModel() => LSequence.Create()
            .AppendInterval(3.25f)
            .Join(_canvas.ModelAnimation.PlayAppearance(5f))
            .AppendInterval(1.2f)
            .JoinAction(() => _audioController.PlaySfx(_audioStorage.ModelAppearance, 0.5f))
            .Run();

        private MotionHandle PlayMenuTheme() => LSequence.Create()
            .JoinAction(() => _audioController.PlayBgm(_audioStorage.TubeAppearance, 0.5f))
            .AppendInterval(_audioStorage.TubeAppearance.length)
            .Join(_audioController.PlayBgmWithPitchFade(_audioStorage.MenuTheme, 1f, 10f))
            .AppendInterval(1.1f)
            .Join(_canvas.ModelAnimation.PlayRotation())
            .Run();
    }
}
