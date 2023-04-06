using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace ReadyPlayerMe.Core
{
    public struct ResponseTexture : IResponse
    {
        public Texture2D Texture;

        public bool IsSuccess { get; private set; }
        public string Error { get; private set; }

        public void Parse(bool isSuccess, UnityWebRequest request)
        {
            IsSuccess = isSuccess;
            if (IsSuccess)
            {
                Texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
            }
            else
            {
                Error = request.error;
            }
        }
    }
}
