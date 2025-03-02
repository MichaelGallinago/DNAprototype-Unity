using Cysharp.Threading.Tasks;
using DnaCore.Audio;
using DnaCore.Utilities;
using LitMotion;
using Scenes.Menu.Audio;
using UnityEngine.UIElements;
using UxmlViewBindings;

//TODO: replace "static (evt, userArgs) => ..." to method group after C# 11 in unity
// ReSharper disable UnusedParameter.Local
namespace Scenes.Menu.OptionCard
{
    public static class CardsUtilities
    {
        public static void RegisterCallbacks(MainMenuArgs args)
        {
            var builder = new CallbackBuilder<MainMenuArgs>(args);
            RegisterCardCallbacks(args.Binding.CardSaves.Button, builder, 
                static (evt, userArgs) => OnSavesPressed(evt, userArgs));
            RegisterCardCallbacks(args.Binding.CardSettings.Button, builder,
                static (evt, userArgs) => OnSettingsPressed(evt, userArgs));
            RegisterCardCallbacks(args.Binding.CardShutdown.Button, builder,
                static (evt, userArgs) => OnShutdownPressed(evt, userArgs));
        }

        public static MotionHandle Show(MainMenuArgs args) => LSequence.Create()
            .AppendInterval(1f)
            .AppendAction(args, motionArgs => SetCardsEnabled(true, motionArgs))
            .AppendInterval(0.1f)
            .AppendAction(args, static motionArgs => PlayCardSound(motionArgs))
            .AppendInterval(0.2f)
            .AppendAction(args, static motionArgs => PlayCardSound(motionArgs))
            .AppendInterval(0.2f)
            .AppendAction(args, static motionArgs => PlayCardSound(motionArgs))
            .RunAfterAction();

        private static void RegisterCardCallbacks(
            Button cardButton,
            CallbackBuilder<MainMenuArgs> builder,
            EventCallback<EventBase, MainMenuArgs> onPressedCallback) => builder
                .Register<MouseEnterEvent>(cardButton,
                    static (evt, userArgs) => SoundUtilities.PlayFocus(evt, userArgs))
                .Register<NavigationSubmitEvent>(cardButton, onPressedCallback)
                .Register<ClickEvent>(cardButton, onPressedCallback);

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