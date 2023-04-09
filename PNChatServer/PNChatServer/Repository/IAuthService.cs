using PNChatServer.Dto;
using PNChatServer.Models;

namespace PNChatServer.Repository
{
    public interface IAuthService
    {
        AccessToken Login(User user);
        void SignUp(User user);
        void PutHubConnection(string userSession, string key);
    }
}
