using Assets._Scripts.Network.Enum;
using Newtonsoft.Json;

namespace Assets._Scripts.Network.Message
{
    class ConnectedToServerMessage : BaseMessage
    {
        [JsonProperty("welcomeMessage", NullValueHandling = NullValueHandling.Ignore)]
        public string welcomeMessage;
        public int playerId;

        public ConnectedToServerMessage() : base(MessageType.Connect)
        {
        }
    }
}
