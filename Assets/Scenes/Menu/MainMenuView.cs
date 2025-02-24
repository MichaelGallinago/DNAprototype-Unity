using LitMotion;
using Scenes.Menu.Audio;
using Scenes.Menu.Logo;
using Scenes.Menu.Model;
using Scenes.Menu.OptionCard;
using Scenes.Menu.Settings;
using Scenes.Menu.Tube;
using UnityEngine;
using UnityEngine.UIElements;
using UxmlViewBindings;

//TODO: replace "static (evt, userArgs) => ..." to method group after C# 11 in unity
namespace Scenes.Menu
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private AudioStorage _audioStorage;
        [SerializeField] private UIDocument _document;
        [SerializeField] private MainMenuCanvas _canvas;
        [SerializeField] private MainMenuViewModel _viewModel;
        
        private MotionHandle _startAnimation;
        
        private void Start()
        {
            var binding = new MainMenuViewBinding(_document.rootVisualElement);
            var args = new MainMenuArgs(in binding, _audioStorage, _canvas);
            CardsUtilities.RegisterCallbacks(args);
            SettingsUtilities.RegisterCallbacks(args);
            
            AnimateStart(args);
        }
        
        private void AnimateStart(MainMenuArgs args)
        {
            args.StartAnimation = LSequence.Create()
                .Join(TubeUtilities.Animate(args))
                .Join(ModelUtilities.Animate(args))
                .Join(LogoUtilities.Show(args))
                .Run();
            
            _ = CardsUtilities.Show(args);
        }
    }
}
