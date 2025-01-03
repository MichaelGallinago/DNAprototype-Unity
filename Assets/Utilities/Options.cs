using System;

namespace Utilities
{
    [Serializable]
    public readonly struct GameOptions
    {
        public const int BaseFrameRate = 60;
        
        public readonly ushort FrameRateLimit;
        public readonly ushort SimulationFrameRate;
        public readonly float SimulationSpeed;

        public GameOptions(ushort frameRateLimit, ushort simulationFrameRate) 
        {
            FrameRateLimit = frameRateLimit;
            SimulationFrameRate = simulationFrameRate;
            SimulationSpeed = (float)simulationFrameRate / BaseFrameRate;
        }
    }
}
