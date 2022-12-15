using Global;
using KnjiznicarDataModel.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Network.MessageHandlers
{
    class SpawnPlayersMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(JObject dataJsonObject)
        {
            SpawnPlayersMessage message = JsonConvert.DeserializeObject<SpawnPlayersMessage>(dataJsonObject.ToString());

            foreach((string, float[]) playerCoordinates in message.spawnArray)
            {
                GlobalGameManager.Instance.SpawnPlayer(playerCoordinates.Item1);
            }
        }
    }
}
