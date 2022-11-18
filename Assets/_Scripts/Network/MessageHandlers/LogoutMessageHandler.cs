using Assets._Scripts.Network.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Assets._Scripts.Network.MessageHandlers
{
    class LogoutMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(JObject dataJsonObject)
        {
            LogoutMessage message = JsonConvert.DeserializeObject<LogoutMessage>(dataJsonObject.ToString());

            if (message.responseNeeded)
            {
                ClientSend.SendTCPData(new LogoutMessage(false), Client.Instance.LoginServer);
            }

            Client.Instance.Disconnect();

            GlobalPlayerData.Instance.username = "";
            GlobalPlayerData.Instance.playerId = -1;
            GlobalPlayerData.Instance.level = -1;
            GlobalPlayerData.Instance.items = null;
            GlobalPlayerData.Instance.adventureLevel = -1;
            GlobalPlayerData.Instance.pvpPoints = -1;
        }
    }
}
