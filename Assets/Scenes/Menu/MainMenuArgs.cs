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
        public MotionHandle StartAnimation;
            
        public MainMenuArgs(in MainMenuViewBinding binding, AudioStorage audioStorage, MainMenuCanvas canvas)
        {
            Binding = binding;
            AudioStorage = audioStorage;
            Canvas = canvas;
        }
    }
}
