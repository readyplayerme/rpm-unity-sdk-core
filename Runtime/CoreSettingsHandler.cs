using ReadyPlayerMe.Core.Data;
using UnityEngine;

namespace ReadyPlayerMe.Core
{
    public static class CoreSettingsHandler
    {
        public static CoreSettings CoreSettings
        {
            get
            {
                if (coreSettings != null) return coreSettings;
                coreSettings = CoreSettings.Load();
                if (coreSettings == null)
                {
                    Debug.LogError("CoreSettings could not be loaded.");
                }
                return coreSettings;
            }
        }

        private static CoreSettings coreSettings;
    }
}
