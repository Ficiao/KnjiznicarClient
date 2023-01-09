using KnjiznicarDataModel.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using UnityEngine;

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
                    Client.SessionId = message.SessionId;
                    Client.OverworldServer.Udp.Connect();
                    break;
                case KnjiznicarDataModel.Enum.ServerType.Instance:
                    break;
                default:
                    break;
            }
        }
    }
}
