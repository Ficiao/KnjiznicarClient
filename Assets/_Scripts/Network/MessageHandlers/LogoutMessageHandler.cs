using Shared;
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

            if (message.ResponseNeeded)
            {
                ClientSend.SendTCPData(new LogoutMessage(false), Client.OverworldServer);
            }
            else
            {
                Client.Instance.DisconnectAll(true);
            }

            GlobalPlayerData.PlayerData.PlayerName = "";
            GlobalPlayerData.PlayerData.PlayerId = "";
            GlobalPlayerData.PlayerData.Level = -1;
            GlobalPlayerData.PlayerData.Items = null;
            GlobalPlayerData.PlayerData.AdventureLevel = -1;
            GlobalPlayerData.PlayerData.PvpPoints = -1;
        }
    }
}
