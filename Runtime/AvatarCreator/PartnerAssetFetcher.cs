using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ReadyPlayerMe.Core;

namespace ReadyPlayerMe.AvatarCreator
{
    public class PartnerAssetFetcher
    {
        private readonly PartnerAssetsRequests partnerAssetsRequests;

        public async Task<PartnerAsset[]> FetchAssetData(Category category, BodyType bodyType, OutfitGender gender, CancellationToken token = default)
        {
            return await partnerAssetsRequests.Get(category, bodyType, gender, token);
        }
    }
}
