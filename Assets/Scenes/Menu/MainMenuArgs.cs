using LitMotion;
using Scenes.Menu.Audio;
using UxmlViewBindings;

namespace Scenes.Menu
{
    public class MainMenuArgs
    {
        public readonly MainMenuViewBinding Binding;
        public readonly AudioStorage AudioStorage;
        public readonly MainMenuCanvas Canvas;
        public readonly MainMenuViewModel ViewModel;
        
        public MotionHandle StartAnimation;
        public long LastSelectSoundTime;
        
        public MainMenuArgs(
            in MainMenuViewBinding binding, 
            AudioStorage audioStorage, 
            MainMenuCanvas canvas,
            MainMenuViewModel viewModel)
        {
            Binding = binding;
            AudioStorage = audioStorage;
            Canvas = canvas;
            ViewModel = viewModel;
        }
    }
}
