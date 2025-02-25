using Cysharp.Threading.Tasks;
using DnaCore.Audio;
using DnaCore.Utilities;
using LitMotion;
using Scenes.Menu.Audio;
using UnityEngine.UIElements;
using UxmlViewBindings;

namespace Scenes.Menu.OptionCard
{
    public static class CardsUtilities
    {
        public static void RegisterCallbacks(MainMenuArgs args)
        {
            RegisterCardCallbacks(in args.Binding.CardSaves, args, 
                static (evt, userArgs) => OnSavesPressed(evt, userArgs));
            RegisterCardCallbacks(in args.Binding.CardSettings, args,
                static (evt, userArgs) => OnSettingsPressed(evt, userArgs));
            RegisterCardCallbacks(in args.Binding.CardShutdown, args,
                static (evt, userArgs) => OnShutdownPressed(evt, userArgs));
        }

        public static MotionHandle Show(MainMenuArgs args) => LSequence.Create()
            .JoinAction(args, motionArgs => SetCardsEnabled(true, motionArgs), 1f)
            .JoinAction(args, static motionArgs => PlayCardSound(motionArgs), 1.1f)
            .JoinAction(args, static motionArgs => PlayCardSound(motionArgs), 1.3f)
            .JoinAction(args, static motionArgs => PlayCardSound(motionArgs), 1.5f)
            .Run();
        
        private static void RegisterCardCallbacks(
            in OptionCardViewBinding binding, 
            MainMenuArgs args, 
            EventCallback<EventBase, MainMenuArgs> onPressedCallback)
        {
            binding.Button.RegisterCallback<MouseEnterEvent, MainMenuArgs>(
                static (evt, userArgs) => SoundUtilities.PlayFocus(evt, userArgs), args);
            binding.Button.RegisterCallback<NavigationSubmitEvent, MainMenuArgs>(onPressedCallback, args);
            binding.Button.RegisterCallback<ClickEvent, MainMenuArgs>(onPressedCallback, args);
        }

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
            SetCardEnabled(in args.Binding.CardSaves, isEnabled);
            SetCardEnabled(in args.Binding.CardSettings, isEnabled);
            SetCardEnabled(in args.Binding.CardShutdown, isEnabled);
        }
        
        private static void SetCardEnabled(in OptionCardViewBinding card, bool isEnabled)
        {
            card.Root.enabledSelf = isEnabled;
            card.Button.enabledSelf = isEnabled;
        }
        
        private static void OnSavesPressed(EventBase e, MainMenuArgs args)
        {
            SoundUtilities.PlaySelect(args);
            _ = Hide(args);
        }
        
        private static void OnSettingsPressed(EventBase e, MainMenuArgs args)
        {
            SoundUtilities.PlaySelect(args);
            _ = Hide(args);
            
            args.Binding.Settings.Root.enabledSelf = true;
        }

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