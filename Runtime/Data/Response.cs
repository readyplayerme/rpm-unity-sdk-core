using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace ReadyPlayerMe.Core
{
    public struct Response : IResponse
    {
        public string Text;
        public byte[] Data;

        public bool IsSuccess { get; private set; }
        public string Error { get; private set; }

        public void Parse(bool isSuccess, UnityWebRequest request)
        {
            IsSuccess = isSuccess;
            if (!IsSuccess)
            {
                Error = request.error;
            }

            if (request.downloadHandler is DownloadHandlerFile)
            {
                return;
            }

            Text = request.downloadHandler.text;
            Data = request.downloadHandler.data;
        }
    }
}
