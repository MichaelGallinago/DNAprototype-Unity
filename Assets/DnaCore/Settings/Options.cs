using System;

namespace DnaCore.Settings
{
    [Serializable]
    public struct Options
    {
        public const int BaseFrameRate = 60;
        public const int MaxSimulationRate = 600;

        public bool FullScreen;
        public AspectRatio AspectRatio;
        public int Scale;
        public int VSync;
        public int TargetFrameRate;
        public int SimulationRate;
        
        public Options(
            bool fullScreen, 
            AspectRatio aspectRatio,
            int scale, 
            int vSync, 
            int targetFrameRate, 
            int simulationRate)
        {
            FullScreen = fullScreen;
            AspectRatio = aspectRatio;
            Scale = scale;
            VSync = vSync;
            TargetFrameRate = targetFrameRate;
            SimulationRate = simulationRate;
        }
    }
}
