using UnityEngine;

namespace Scenes.Bootstrap
{
    public static class Settings
    {
        public static Options Options = new(Options.BaseFrameRate, Options.BaseFrameRate);

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Bootstrap()
        {
            //TODO: Load config
        }
    }
}
