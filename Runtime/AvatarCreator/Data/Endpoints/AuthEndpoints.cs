namespace ReadyPlayerMe.AvatarCreator
{
    public abstract class AuthEndpoints
    {
        public static string GetAuthAnonymousEndpoint(string subdomain)
        {
            return string.Format(Env.RPM_API_SUBDOMAIN_URL, subdomain, "/users");
        }

        public static string GetAuthStartEndpoint(string subdomain)
        {
            return string.Format(Env.RPM_API_SUBDOMAIN_URL, subdomain, "/auth/start");
        }

        public static string GetConfirmCodeEndpoint(string subdomain)
        {
            return string.Format(Env.RPM_API_SUBDOMAIN_URL, subdomain, "/auth/login");
        }

        public static string GetTokenRefreshEndpoint(string subdomain)
        {
            return string.Format(Env.RPM_API_SUBDOMAIN_URL, subdomain, "/auth/refresh");
        }
    }
}
