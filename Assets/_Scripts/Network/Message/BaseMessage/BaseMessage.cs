using Assets._Scripts.Network.Enum;

namespace Assets._Scripts.Network.Message
{
    class BaseMessage
    {
        public MessageType messageType { get; }

        protected BaseMessage(MessageType type)
        {
            messageType = type;
        }
    }
}
