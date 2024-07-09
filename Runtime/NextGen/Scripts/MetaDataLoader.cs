using System.Threading;
using System.Threading.Tasks;
using ReadyPlayerMe.Core;
using UnityEngine;

namespace ReadyPlayerMe.NextGen
{
    public class MetaDataLoader
    {
        public int Timeout { get; set; } = 20;
        private const string METADATA_TIME_FORMAT = "yyyy-MM-dd'T'HH:mm:ss.fff'Z'";
        private readonly string avatarUrl;
        private readonly CancellationToken token;

        public MetaDataLoader(string url, CancellationToken token = new CancellationToken())
        {
            avatarUrl = url;
            this.token = token;
        }

        public async Task<AvatarMetaData> Load()
        {
            var metaData = await LoadAvatarMetaData();
            var avatarMetaData = ParseMetaData(metaData);
            return avatarMetaData;
        }

        private AvatarMetaData ParseMetaData(byte[] metaData)
        {
            if (metaData == null)
            {
                return new AvatarMetaData();
            }

            var json = System.Text.Encoding.UTF8.GetString(metaData);
            Debug.Log($"JSON: {json}");
            var dataWrapper = JsonUtility.FromJson<MetaDataResponseWrapper>(json);
            return dataWrapper.data.GetMetaData();
        }

        private async Task<byte[]> LoadAvatarMetaData()
        {
            var dispatcher = new WebRequestDispatcher();
            try
            {
                Debug.Log($"Loading from url: {avatarUrl}");
                var response = await dispatcher.DownloadIntoMemory(avatarUrl, token, Timeout);
                return response.Data;
            }
            catch (CustomException exception)
            {
                if (exception.FailureType == FailureType.NoInternetConnection)
                {
                    throw;
                }
                Debug.LogError($"Failed to download glb model into memory. {exception}");
                return null;
            }
        }
    }
}
