using System;

namespace Scenes.Bootstrap
{
    [Serializable]
    public readonly struct Options
    {
        public const ushort BaseFrameRate = 60;
        
        public readonly ushort FrameRateLimit;
        public readonly ushort SimulationRate;
        
        public Options(ushort frameRateLimit, ushort simulationRate)
        {
            FrameRateLimit = frameRateLimit;
            SimulationRate = simulationRate;
        }
    }
}
