using Assets._Scripts.Network.Enum;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Assets._Scripts.Network.Message
{
    class LoginSuccessfulMessage : BaseMessage
    {
        public bool loginSuccessful;
        public bool isLogin;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PlayerData playerData;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string overworldIp;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int overworldPort;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string instanceIp;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int instancePort;

        public LoginSuccessfulMessage() : base(MessageType.LoginSuccessful)
        {
        }
    }
}
