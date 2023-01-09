using KnjiznicarDataModel.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared;

namespace Network.MessageHandlers
{
    class PvpLeaderboardUpdateMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(JObject dataJsonObject)
        {
            PvpLeaderboardUpdateMessage message = JsonConvert.DeserializeObject<PvpLeaderboardUpdateMessage>(dataJsonObject.ToString());

            GlobalGameManager.Instance.UpdatePvpLeaderboard(message.Leaderboard);
        }
    }
}
