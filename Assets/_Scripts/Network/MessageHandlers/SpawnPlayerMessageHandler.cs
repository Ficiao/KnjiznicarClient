using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using KnjiznicarDataModel.Message;
using Shared;

namespace Network.MessageHandlers
{
    class SpawnPlayerMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(JObject dataJsonObject)
        {
            SpawnPlayerMessage message = JsonConvert.DeserializeObject<SpawnPlayerMessage>(dataJsonObject.ToString());

            GlobalGameManager.Instance.SpawnPlayer(message.PlayerUsername);
            GlobalGameManager.Instance.UpdatePvpLeaderboard(message.PvpLeaderboard);
            GlobalGameManager.Instance.UpdateAdventureLeaderboard(message.AdventureLeaderboard);
        }
    }
}
