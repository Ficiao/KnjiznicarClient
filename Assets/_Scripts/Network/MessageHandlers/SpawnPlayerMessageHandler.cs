using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using KnjiznicarDataModel.Message;

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
