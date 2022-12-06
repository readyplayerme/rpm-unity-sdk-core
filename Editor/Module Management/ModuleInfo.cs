using System;
using UnityEditor.PackageManager;

namespace ReadyPlayerMe.Core.Editor
{
    [Serializable]
    public struct ModuleInfo
    {
        public string name;
        public string gitUrl;
        public string branch;
        
        public string Identifier
        {
            get
            {
                if (gitUrl == string.Empty)
                {
                    return name;
                }

                return gitUrl + (string.IsNullOrEmpty(branch) ? string.Empty : $"#{branch}" );
            }
        }
    }
}
