using System.Threading.Tasks;
using NUnit.Framework;
using ReadyPlayerMe.AvatarCreator;

namespace ReadyPlayerMe.Core.Tests
{
    public class AuthTests
    {
        [Test]
        public async Task Login_As_Anonymous()
        {
            await AuthManager.LoginAsAnonymous();
            Assert.False(string.IsNullOrEmpty(AuthManager.UserSession.Id));
        }
    }
}
