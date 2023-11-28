using System.Collections.Generic;

namespace ReadyPlayerMe.AvatarCreator.Avatars
{
    public class AvatarTemplateResponse
    {
        public List<AvatarTemplate> Data { get; set; }
    }

    public class AvatarTemplate
    {
        public string Id { get; set; }
        
        public string Gender { get; set; }
        
        public string ImageUrl { get; set; }
    }
}
