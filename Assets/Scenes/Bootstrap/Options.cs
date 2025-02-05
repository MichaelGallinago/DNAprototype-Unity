using System;

namespace Scenes.Bootstrap
{
    [Serializable]
    public readonly struct Options
    {
        public const ushort BaseFrameRate = 60;
        
        public readonly ushort FrameRateLimit;
        public readonly ushort SimulationFrameRate;
        
        public Options(ushort frameRateLimit, ushort simulationFrameRate)
        {
            FrameRateLimit = frameRateLimit;
            SimulationFrameRate = simulationFrameRate;
        }
    }
}
