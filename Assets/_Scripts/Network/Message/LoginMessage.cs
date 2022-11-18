using Assets._Scripts.Network.Enum;

namespace Assets._Scripts.Network.Message
{
    class LoginMessage : BaseMessage
    {

        public string username;
        public string passwordHash;

        public LoginMessage() : base(MessageType.Login)
        {
        }        
    }
}
