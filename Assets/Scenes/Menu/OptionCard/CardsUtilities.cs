using Cysharp.Threading.Tasks;
using DnaCore.Singletons.Audio;
using DnaCore.Utilities;
using LitMotion;
using Scenes.Menu.Audio;
using Scenes.Menu.Settings;
using UnityEngine.UIElements;
using UxmlViewBindings;

//TODO: replace "static (evt, userArgs) => ..." to method group after C# 11 in unity
// ReSharper disable UnusedParameter.Local
namespace Scenes.Menu.OptionCard
{
    public static class CardsUtilities
    {
        public static void RegisterCallbacks(MainMenuArgs args) => new CallbackBuilder<MainMenuArgs>(args)
            .SetTarget(args.Binding.CardSaves.Button)
            .Register<FocusInEvent>(static (evt, userArgs) => SoundUtilities.PlayFocus(evt, userArgs))
            .Register<ClickEvent>(static (evt, userArgs) => OnSavesPressed(evt, userArgs))
            .Register<NavigationSubmitEvent>(static (evt, userArgs) => OnSavesPressedFocused(evt, userArgs))
            
            .SetTarget(args.Binding.CardSettings.Button)
            .Register<FocusInEvent>(static (evt, userArgs) => SoundUtilities.PlayFocus(evt, userArgs))
            .Register<ClickEvent>(static (evt, userArgs) => OnSettingsPressed(evt, userArgs))
            .Register<NavigationSubmitEvent>(static (evt, userArgs) => OnSettingsPressedFocused(evt, userArgs))
            
            .SetTarget(args.Binding.CardShutdown.Button)
            .Register<FocusInEvent>(static (evt, userArgs) => SoundUtilities.PlayFocus(evt, userArgs))
            .Register<ClickEvent>(static (evt, userArgs) => OnShutdownPressed(evt, userArgs))
            .Register<NavigationSubmitEvent>(static (evt, userArgs) => OnShutdownPressed(evt, userArgs));

        public static MotionHandle Show(MainMenuArgs args, float delay = 0f) => 
            LSequence.Create()
                .AppendInterval(delay)
                .AppendAction(args, motionArgs => SetCardsEnabled(true, motionArgs))
                .AppendInterval(0.1f)
                .AppendAction(args, static motionArgs => PlayCardSound(motionArgs))
                .AppendInterval(0.2f)
                .AppendAction(args, static motionArgs => PlayCardSound(motionArgs))
                .AppendInterval(0.2f)
                .AppendAction(args, static motionArgs => PlayCardSound(motionArgs))
                .RunAfterAction();

        private static async UniTask Hide(MainMenuArgs args)
        {
            SetCardsEnabled(false, args);
            
            PlayCardSound(args);
            await UniTask.WaitForSeconds(0.2f);
            PlayCardSound(args);
            await UniTask.WaitForSeconds(0.2f);
            PlayCardSound(args);
        }
        
        private static void PlayCardSound(MainMenuArgs args) => 
            AudioPlayer.PlaySfx(args.AudioStorage.CardMovement, 0.1f);
        
        private static void SetCardsEnabled(bool isEnabled, MainMenuArgs args)
        {
            args.Binding.CardSaves.Root.enabledSelf = isEnabled;
            args.Binding.CardSettings.Root.enabledSelf = isEnabled;
            args.Binding.CardShutdown.Root.enabledSelf = isEnabled;
        }
        
        private static void OnSavesPressed(EventBase e, MainMenuArgs args, bool withFocus = false)
        {
            SoundUtilities.PlaySelect(args);
            _ = Hide(args);
        }
        
        private static void OnSavesPressedFocused(EventBase e, MainMenuArgs args) => 
            OnSavesPressed(e, args, true);
        
        private static void OnSettingsPressed(EventBase e, MainMenuArgs args, bool withFocus = false)
        {
            SettingsUtilities.Open(args, withFocus);
            SoundUtilities.PlaySelect(args);
            _ = Hide(args);
        }
        
        private static void OnSettingsPressedFocused(EventBase e, MainMenuArgs args) => 
            OnSettingsPressed(e, args, true);

        private static void OnShutdownPressed(EventBase e, MainMenuArgs args)
        {
            if (args.StartAnimation.IsPlaying())
            {
                args.StartAnimation.Cancel();
            }
            
            SoundUtilities.PlaySelect(args);
            _ = Hide(args);
            _ = TransitionUtilities.Quit(args);
        }
    }
}