using Assets._Scripts.Network.Enum;

namespace Assets._Scripts.Network.Message
{
    class PlayerInputMessage : BaseMessage
    {
        public int leftRightDirection;
        public int forwardDirection;
        public bool jump;

        public PlayerInputMessage() : base(MessageType.PlayerInput)
        {
        }
    }
}
