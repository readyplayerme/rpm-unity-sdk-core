using UnityEngine;

namespace ReadyPlayerMe.AvatarCreator
{
    public abstract class AssetEndpoints
    {
        private static readonly string ASSET_ENDPOINT = Env.RPM_API_V1_URL + "assets?limit={0}&page={1}&filter=viewable-by-user-and-app&filterUserId={2}&filterApplicationId={3}&gender=neutral&gender={4}";

        public static string GetAssetEndpoint(string type, int limit, int page, string userId, string appId, string gender)
        {
            Debug.Log(Env.RPM_API_V1_URL);
            
            if (string.IsNullOrEmpty(type))
            {
                return string.Format(ASSET_ENDPOINT, limit, page, userId, appId, gender);
            }

            return string.Format(ASSET_ENDPOINT, limit, page, userId, appId, gender) + "&type=" + type;
        }
    }
}
