using Assets._Scripts.Network.Enum;

namespace Assets._Scripts.Network.Message
{
    class LogoutMessage : BaseMessage
    {
        public bool responseNeeded;

        public LogoutMessage(bool responseNeeded) : base(MessageType.Logout)
        {
            this.responseNeeded = responseNeeded;
        }
    }
}
