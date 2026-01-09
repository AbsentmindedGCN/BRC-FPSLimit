using BepInEx.Configuration;

namespace SilentFPSLimit
{
    public class SilentFPSLimitConfig
    {
        public ConfigEntry<int> FPSLimit;
        public ConfigEntry<bool> EnableVSync;

        public SilentFPSLimitConfig(ConfigFile config)
        {
            FPSLimit = config.Bind(
                "FPS",
                "FPSLimit",
                120,
                "Target FPS limit. Use -1 for unlimited."
            );

            EnableVSync = config.Bind(
                "FPS",
                "EnableVSync",
                false,
                "Enable VSync. If enabled, FPS Limit is ignored. Leave this false."
            );
        }
    }
}
