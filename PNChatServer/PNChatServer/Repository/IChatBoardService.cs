using PNChatServer.Dto;

namespace PNChatServer.Repository
{
    public interface IChatBoardService
    {
        List<GroupDto> GetHistory(string userSession);
        object GetInfo(string userSession, string groupCode, string contactCode);
        void AddGroup(string userCode, GroupDto group);
        GroupDto UpdateGroupAvatar(GroupDto group);
        void SendMessage(string userCode, string groupCode, MessageDto message);
        List<MessageDto> GetMessageByGroup(string userCode, string groupCode);
        List<MessageDto> GetMessageByContact(string userCode, string contactCode);
    }
}
