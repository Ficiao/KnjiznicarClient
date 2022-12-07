using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using KnjiznicarDataModel.Message;
using Global;

namespace Network.MessageHandlers
{
    class SpawnPlayerMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(JObject dataJsonObject)
        {
            SpawnPlayerMessage message = JsonConvert.DeserializeObject<SpawnPlayerMessage>(dataJsonObject.ToString());

            GlobalGameManager.Instance.SpawnPlayer(message.playerId, message.playerUsername);
        }
    }
}
