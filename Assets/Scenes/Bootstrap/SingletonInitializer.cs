using DnaCore.Audio;
using UnityEngine;

namespace Scenes.Bootstrap
{
    public static class SingletonInitializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeSingletons()
        {
            AudioPlayerInstance.Initialize();
        }
    }
}