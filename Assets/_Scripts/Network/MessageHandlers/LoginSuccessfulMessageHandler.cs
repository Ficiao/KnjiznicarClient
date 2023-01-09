using KnjiznicarDataModel.Message;
using Login;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared;

namespace Network.MessageHandlers
{
    class LoginSuccessfulMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(JObject dataJsonObject)
        {
            LoginSuccessfulMessage message = JsonConvert.DeserializeObject<LoginSuccessfulMessage>(dataJsonObject.ToString());

            if (message.LoginSuccessful && message.IsLogin)
            {
                GlobalPlayerData.PlayerData = message.PlayerData;
                Client.Instance.ConnectToWorldServer(message.OverworldIp, message.OverworldPort);

                UIManager.Instance.LoggedIn();
                Client.LoginServer.Disconnect(false);
            }
            else if (message.LoginSuccessful)
            {
                UIManager.Instance.ShowNameSelection(message.Username);
            }
        }
    }
}
