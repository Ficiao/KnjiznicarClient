using Global;
using KnjiznicarDataModel.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Network.MessageHandlers
{
    class LogoutMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(JObject dataJsonObject)
        {
            LogoutMessage message = JsonConvert.DeserializeObject<LogoutMessage>(dataJsonObject.ToString());

            if (message.responseNeeded)
            {
                ClientSend.SendTCPData(new LogoutMessage(false), Client.OverworldServer);
            }
            else
            {
                Client.Instance.DisconnectAll(true);
            }

            GlobalPlayerData.Instance.PlayerData.playerName = "";
            GlobalPlayerData.Instance.PlayerData.playerId = "";
            GlobalPlayerData.Instance.PlayerData.level = -1;
            GlobalPlayerData.Instance.PlayerData.items = null;
            GlobalPlayerData.Instance.PlayerData.adventureLevel = -1;
            GlobalPlayerData.Instance.PlayerData.pvpPoints = -1;
        }
    }
}
