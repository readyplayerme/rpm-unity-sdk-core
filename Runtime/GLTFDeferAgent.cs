using GLTFast;
using UnityEngine;

namespace ReadyPlayerMe
{
    public class GLTFDeferAgent : MonoBehaviour
    {
        public IDeferAgent GetGLTFastDeferAgent()
        {
            return GetComponent<IDeferAgent>();
        }
    }
}
