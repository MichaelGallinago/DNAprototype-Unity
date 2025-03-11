using DnaCore.Settings;
using LitMotion;
using Scenes.Menu.Audio;
using Scenes.Menu.OptionCard;
using Scenes.Menu.Saves;
using Scenes.Menu.Settings;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UxmlViewBindings;

namespace Scenes.Menu
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private AudioStorage _audioStorage;
        [SerializeField] private UIDocument _document;
        [SerializeField] private MainMenuCanvas _canvas;
        [SerializeField] private CanvasScaler _canvasScaler;
        [SerializeField] private MainMenuViewModel _viewModel;
        
        private MotionHandle _startAnimation;
        
        private void Start()
        {
            var binding = new MainMenuViewBinding(_document.rootVisualElement);
            var args = new MainMenuArgs(in binding, _audioStorage, _canvas, _viewModel);
            
            CardsUtilities.RegisterCallbacks(args);
            SettingsUtilities.Initialize(args);
            SavesUtilities.RegisterCallbacks(args);
            
            TransitionUtilities.Enter(args);
            
            _document.panelSettings.SetScreenToPanelSpaceFunction(
                static coords => TransformScreenToPanelSpace(coords));
        }

        private static Vector2 TransformScreenToPanelSpace(Vector2 screenPosition) =>
            new Vector2(screenPosition.x / Screen.width, screenPosition.y / Screen.height) 
                * AppSettings.Options.AspectRatio.MinResolution;
    }
}
