using Newtonsoft.Json.Linq;
using UnityEngine;

namespace ReadyPlayerMe.AvatarCreator
{
    public static class EnvLoader
    {
        private const string LOCAL_ENV_FILENAME = "env.local";
        
        public static string LoadVar(string varName, string defaultValue)
        {
            var jsonContent = Resources.Load<TextAsset>(LOCAL_ENV_FILENAME);

            var loadedEnv = JObject.Parse(jsonContent.text);

            return loadedEnv[varName]?.ToString() ?? defaultValue;
        }
    }
}
