using System.Runtime.CompilerServices;

namespace DnaCore.Settings
{
    public static class AppSettings
    {
        public static AppSettingsInstance Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)] get => AppSettingsInstance.Instance;
        }

        public static ref Options Options
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)] get => ref Instance.Options;
        }

        public static ref Audio Audio
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)] get => ref Instance.Audio;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Save() => AppSettingsInstance.Save();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Load() => AppSettingsInstance.Load();
    }
}
