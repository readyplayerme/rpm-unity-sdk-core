using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace ReadyPlayerMe.Core
{
    public class ResponseFile : IResponse
    {
        public bool IsSuccess { get; private set; }
        public string Error { get; private set; }

        public byte[] Data { get; private set; }

        private ulong length;

        public void Parse(bool isSuccess, UnityWebRequest request)
        {
            IsSuccess = isSuccess;
            if (!IsSuccess)
            {
                Error = request.error;
            }

            length = request.downloadedBytes;
        }

        public async Task ReadFile(string path, CancellationToken token)
        {
            var byteLength = (long) length;
            var info = new FileInfo(path);

            while (info.Length != byteLength && !token.IsCancellationRequested)
            {
                info.Refresh();
                await Task.Yield();
            }

            if (token.IsCancellationRequested)
            {
                return;
            }

            // Reading file since can't access raw bytes from downloadHandler
            Data = File.ReadAllBytes(path);
        }
    }
}
