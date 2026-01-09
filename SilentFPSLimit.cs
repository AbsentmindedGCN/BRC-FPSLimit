using BepInEx;
using UnityEngine;

namespace SilentFPSLimit
{
    [BepInPlugin("silent.fpslimit", "FPS Limit", "1.0.0")]
    public class SilentFPSLimitPlugin : BaseUnityPlugin
    {
        internal static SilentFPSLimitConfig ConfigData;

        private int _lastSetTargetFrameRate = -1;
        private int _lastSetVSyncCount = -1;

        // Timer to ignore external changes during game boot, prevent override by BRC
        private float _initializationGracePeriod = 5f;

        private void Start()
        {
            Logger.LogInfo("Silent FPS Limit Plugin Loaded!");
            ConfigData = new SilentFPSLimitConfig(Config);

            // Set initial state from FPS Limit Config
            SyncToConfig();

            //Logger.LogInfo("FPS Limit Plugin Started. Forcing config for 5 seconds...");
            Logger.LogInfo($"FPS Limit Enabled | Target = {ConfigData.FPSLimit.Value} | VSync = {(ConfigData.EnableVSync.Value)}");
        }

        private void Update()
        {
            // Count down the grace period
            if (_initializationGracePeriod > 0)
            {
                _initializationGracePeriod -= Time.deltaTime;

                // During boot, ignore what the game is doing and force the FPS Limit config
                SyncToConfig();
                ApplySettings();
                return;
            }

            // After 5 seconds, we start checking for external changes
            if (Application.targetFrameRate != _lastSetTargetFrameRate ||
                QualitySettings.vSyncCount != _lastSetVSyncCount)
            {
                _lastSetTargetFrameRate = Application.targetFrameRate;
                _lastSetVSyncCount = QualitySettings.vSyncCount;

                Logger.LogInfo($"External change detected! Now enforcing: {_lastSetTargetFrameRate} FPS.");
            }

            ApplySettings();
        }

        private void SyncToConfig()
        {
            _lastSetVSyncCount = ConfigData.EnableVSync.Value ? 1 : 0;
            _lastSetTargetFrameRate = ConfigData.EnableVSync.Value ? -1 : ConfigData.FPSLimit.Value;
        }

        private void ApplySettings()
        {
            QualitySettings.vSyncCount = _lastSetVSyncCount;
            Application.targetFrameRate = _lastSetTargetFrameRate;
        }
    }
}