using System;
using UnityEngine;

namespace Scenes.Menu
{
    public class MainMenuViewModel : MonoBehaviour
    {
        //TODO: this
        public int Resolution
        {
            set
            {
                //Screen.SetResolution(width, height, Screen.fullScreenMode);
            }
        }

        public int VSync
        {
            set
            {
                //QualitySettings.vSyncCount = Math.Clamp(value, 0, 2);
            }
        }

        public int FrameRate
        {
            set {}
        }

        public int SimulationRate
        {
            set
            {
            }
        }
    }
}
