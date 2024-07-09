using System.Threading;
using System.Threading.Tasks;
using ReadyPlayerMe.Core;
using UnityEngine;

namespace ReadyPlayerMe.NextGen
{
    public class AvatarObjectLoader
    {
        public int Timeout { get; set; } = 20;

        private const string BONE_HIPS = "Hips";
        private const string BONE_ARMATURE = "Armature";
        private readonly string url;
        private readonly CancellationToken token;

        public AvatarObjectLoader(string url, CancellationToken token = new CancellationToken())
        {
            this.url = url;
            this.token = token;
        }

        public async Task<GameObject> LoadAvatar()
        {
            var urlWithoutGlb = url.Replace(".glb", "");
            var metaDataLoader = new MetaDataLoader(urlWithoutGlb, token);
            var metaData = await metaDataLoader.Load();
            var avatarBytes = await LoadAvatarData();
            var gltfImporter = new GltfImporter(token);
            var avatar = await gltfImporter.Import(avatarBytes);
            var avatarData = avatar.AddComponent<AvatarData>();
            avatarData.SetMetaData(metaData);
            ProcessAvatar(avatar);
            avatar.name = metaData.AvatarId;
            avatar.SetActive(true);
            return avatar;
        }

        private async Task<byte[]> LoadAvatarData()
        {
            var dispatcher = new WebRequestDispatcher();
            try
            {
                var response = await dispatcher.DownloadIntoMemory(url, token, Timeout);
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

        private void ProcessAvatar(GameObject avatar)
        {
            if (!avatar.transform.Find(BONE_ARMATURE))
            {
                AddArmatureBone(avatar);
            }
            var animator = avatar.GetComponent<Animator>();
            if (animator == null)
            {
                animator = avatar.AddComponent<Animator>();
            }
        }

        private void AddArmatureBone(GameObject avatar)
        {
            var armature = new GameObject();
            armature.name = BONE_ARMATURE;
            armature.transform.parent = avatar.transform;

            var hips = avatar.transform.Find(BONE_HIPS);
            if (hips) hips.parent = armature.transform;
        }
    }
}
