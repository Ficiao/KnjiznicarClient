using KnjiznicarDataModel.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Shared;

namespace Network.MessageHandlers
{
    class ConnectedToServerMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(JObject dataJsonObject)
        {
            ConnectedToServerMessage message = JsonConvert.DeserializeObject<ConnectedToServerMessage>(dataJsonObject.ToString());
            Debug.Log(message.WelcomeMessage);

            switch (message.ServerType)
            {
                case KnjiznicarDataModel.Enum.ServerType.Login:
                    break;
                case KnjiznicarDataModel.Enum.ServerType.Overworld:
                    if(Client.Instance.GameVersion != message.Version)
                    {
                        GlobalGameManager.Instance.IsWrongVersion = true;
                        Client.Instance.DisconnectAll(true);
                    }
                    else
                    {
                        Client.SessionId = message.SessionId;
                        Client.OverworldServer.Udp.Connect();
                    }
                    break;
                case KnjiznicarDataModel.Enum.ServerType.Instance:
                    break;
                default:
                    break;
            }
        }
    }
}
