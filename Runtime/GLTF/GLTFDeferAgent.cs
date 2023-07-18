#if GLTFAST
using GLTFast;
#endif
using UnityEngine;

namespace ReadyPlayerMe.Core
{
    public class GLTFDeferAgent : MonoBehaviour
    {
#if GLTFAST
        public IDeferAgent GetGLTFastDeferAgent()
        {
            return GetComponent<IDeferAgent>();
        }
#endif
    }
}
