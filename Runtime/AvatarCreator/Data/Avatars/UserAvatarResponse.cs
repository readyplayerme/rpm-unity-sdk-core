using System.Collections.Generic;

namespace ReadyPlayerMe.AvatarCreator.Avatars
{
    public class UserAvatarResponse
    {
        public List<UserAvatar> Data { get; set; }
    }

    public class UserAvatar
    {
        public string Partner { get; set; }
        
        public string Id { get; set; }
    }
}
