using System.IO;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace ReadyPlayerMe.Core
{
    public struct ResponseFile : IResponse
    {
        public bool IsSuccess { get; private set; }
        public string Error { get; private set; }

        public byte[] Data { get; private set; }

        public string Path { get; set; }

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

        public async void ReadFile()
        {
            var byteLength = (long) length;
            var info = new FileInfo(Path);

            while (info.Length != byteLength)
            {
                info.Refresh();
                await Task.Yield();
            }

            // Reading file since can't access raw bytes from downloadHandler
            Data = File.ReadAllBytes(Path);
        }
    }
}
