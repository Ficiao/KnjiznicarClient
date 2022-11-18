using Assets._Scripts.Network.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets._Scripts.Network.MessageHandlers
{
    class LoginSuccessfulMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(JObject dataJsonObject)
        {
            LoginSuccessfulMessage message = JsonConvert.DeserializeObject<LoginSuccessfulMessage>(dataJsonObject.ToString());

            if (message.loginSuccessful)
            {
                Client.Instance.ConnectToWorldServer(message.overworldIp, message.overworldPort);

                GlobalPlayerData.Instance.username = message.playerData.username;
                GlobalPlayerData.Instance.playerId = message.playerData.playerId;
                GlobalPlayerData.Instance.level = message.playerData.level;
                GlobalPlayerData.Instance.items = message.playerData.items;
                GlobalPlayerData.Instance.adventureLevel = message.playerData.adventureLevel;
                GlobalPlayerData.Instance.pvpPoints = message.playerData.pvpPoints;
                UIManager.Instance.LoggedIn(message.playerData.username);
            }
            else
            {
                if (message.isLogin)
                {
                    UIManager.Instance.ShowNotification("Login unssucessful, wrong credentials.");
                }
                else
                {
                    UIManager.Instance.ShowNotification("Register unssucessful, username already taken.");
                }
            }
        }
    }
}
