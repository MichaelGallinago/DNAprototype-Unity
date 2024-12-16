using System;

namespace Utilities
{
    [Serializable]
    public readonly struct GameOptions
    {
        public readonly ushort FrameRateLimit;
        public readonly ushort SimulationFrameRate;

        public GameOptions(
            ushort frameRateLimit, 
            ushort simulationFrameRate) 
        {
            FrameRateLimit = frameRateLimit;
            SimulationFrameRate = simulationFrameRate;
        }
    }
}
