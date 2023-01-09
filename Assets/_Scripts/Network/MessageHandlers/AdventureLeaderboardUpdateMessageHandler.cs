using KnjiznicarDataModel.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared;

namespace Network.MessageHandlers
{
    class AdventureLeaderboardUpdateMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(JObject dataJsonObject)
        {
            AdventureLeaderboardUpdateMessage message = JsonConvert.DeserializeObject<AdventureLeaderboardUpdateMessage>(dataJsonObject.ToString());

            GlobalGameManager.Instance.UpdateAdventureLeaderboard(message.Leaderboard);
        }
    }
}
