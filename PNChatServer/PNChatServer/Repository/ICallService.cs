using PNChatServer.Dto;

namespace PNChatServer.Repository
{
    public interface ICallService
    {
        List<GroupCallDto> GetCallHistory(string userSession);
        List<CallDto> GetHistoryById(string userSession, string groupCallCode);
        string Call(string userSession, string callTo);
        void JoinVideoCall(string userSession, string url);
        void CancelVideoCall(string userSession, string url);

    }
}
