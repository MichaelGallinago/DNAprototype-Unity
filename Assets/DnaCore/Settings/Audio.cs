using System;

namespace DnaCore.Settings
{
    [Serializable]
    public struct Audio
    {
        public const int BaseVolume = 25;
        
        public int SfxVolume;
        public int BgmVolume;
        
        public Audio(int sfxVolume, int bgmVolume)
        {
            SfxVolume = sfxVolume;
            BgmVolume = bgmVolume;
        }
    }
}