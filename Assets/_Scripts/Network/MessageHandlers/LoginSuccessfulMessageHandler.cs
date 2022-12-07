using KnjiznicarDataModel.Message;
using Login;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Global;

namespace Network.MessageHandlers
{
    class LoginSuccessfulMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(JObject dataJsonObject)
        {
            LoginSuccessfulMessage message = JsonConvert.DeserializeObject<LoginSuccessfulMessage>(dataJsonObject.ToString());

            if (message.loginSuccessful && message.isLogin)
            {
                Client.Instance.ConnectToWorldServer(message.overworldIp, message.overworldPort);

                GlobalPlayerData.Instance.PlayerData = message.playerData;
                UIManager.Instance.LoggedIn();
                Client.LoginServer.Disconnect(false);
            }
            else if (message.loginSuccessful)
            {
                UIManager.Instance.ShowNameSelection(message.username);
            }
        }
    }
}
