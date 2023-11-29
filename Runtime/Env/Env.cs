namespace ReadyPlayerMe.AvatarCreator
{
    public static class Env
    {
        public static string RPM_API_SUBDOMAIN_URL => EnvLoader.LoadVar(nameof(RPM_API_SUBDOMAIN_URL), "https://{0}.readyplayer.me/api{1}");
        public static string RPM_API_V2_URL => EnvLoader.LoadVar(nameof(RPM_API_V2_URL), "https://api.readyplayer.me/v2/");
        public static string RPM_API_V1_URL => EnvLoader.LoadVar(nameof(RPM_API_V1_URL), "https://api.readyplayer.me/v1/");
    }
}
