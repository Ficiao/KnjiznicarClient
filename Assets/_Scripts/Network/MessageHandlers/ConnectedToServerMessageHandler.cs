using KnjiznicarDataModel.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Network.MessageHandlers
{
    class ConnectedToServerMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(JObject dataJsonObject)
        {
            ConnectedToServerMessage message = JsonConvert.DeserializeObject<ConnectedToServerMessage>(dataJsonObject.ToString());
            Debug.Log(message.welcomeMessage);
        }
    }
}
