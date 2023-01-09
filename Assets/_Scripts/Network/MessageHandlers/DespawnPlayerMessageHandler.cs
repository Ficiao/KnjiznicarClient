using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using KnjiznicarDataModel.Message;
using Shared;

namespace Network.MessageHandlers
{
    class DespawnPlayerMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(JObject dataJsonObject)
        {
            DespawnPlayerMessage message = JsonConvert.DeserializeObject<DespawnPlayerMessage>(dataJsonObject.ToString());

            GlobalGameManager.Instance.DespawnPlayer(message.PlayerUsername);
        }
    }
}
