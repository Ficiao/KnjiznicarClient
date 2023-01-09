using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using KnjiznicarDataModel.Message;
using Shared;
using System;
using KnjiznicarDataModel.Enum;

namespace Network.MessageHandlers
{
    class PlayerChatMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(JObject dataJsonObject)
        {
            PlayerChatMessage message = JsonConvert.DeserializeObject<PlayerChatMessage>(dataJsonObject.ToString());

            string text = $"[{DateTime.Now.Hour}:{DateTime.Now.Minute}][{message.PlayerName}] {message.Message}";

            ChatController.Instance?.ShowMessage(text);
            switch (GlobalGameManager.Instance.Server)
            {
                case ServerType.Login:
                    break;
                case ServerType.Overworld:
                    if(Overworld.GameManager.Instance.Players.ContainsKey(message.PlayerName))
                    {
                        Overworld.GameManager.Instance.Players[message.PlayerName].ShowText(message.Message);
                    }
                    break;
                case ServerType.Instance:

                    break;
                default:
                    break;
            }
        }
    }
}
