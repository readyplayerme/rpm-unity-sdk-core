<<<<<<< HEAD:Editor/Module Management/ModuleInfo.cs
﻿using System;

namespace ReadyPlayerMe.Core.Editor
=======
﻿namespace ReadyPlayerMe.Core.Editor
>>>>>>> develop:Editor/ModuleInfo.cs
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
