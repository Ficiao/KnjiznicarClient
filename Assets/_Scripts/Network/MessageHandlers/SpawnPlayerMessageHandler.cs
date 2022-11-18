using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using Assets._Scripts.Network.Message;
using Assets._Scripts.Scriptables;

namespace Assets._Scripts.Network.MessageHandlers
{
    class SpawnPlayerMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(JObject dataJsonObject)
        {
            SpawnPlayerMessage message = JsonConvert.DeserializeObject<SpawnPlayerMessage>(dataJsonObject.ToString());

            GameManager.Instance.SpawnPlayer(message.playerId, message.playerUsername);
        }
    }
}
