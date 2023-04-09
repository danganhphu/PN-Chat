using PNChatServer.Dto;

namespace PNChatServer.Repository
{
    public interface IUserService
    {
        UserDto GetProfile(string userCode);
        UserDto UpdateProfile(string userCode, UserDto user);
        List<UserDto> GetContact(string userCode);
        List<UserDto> SearchContact(string userCode, string keySearch);
        void AddContact(string userCode, UserDto user);
    }
}
