using KnjiznicarDataModel.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MatchInstance;

namespace Network.MessageHandlers
{
    class TurnOverMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(JObject dataJsonObject)
        {
            TurnOverMessage message = JsonConvert.DeserializeObject<TurnOverMessage>(dataJsonObject.ToString());

            GameManager.Instance.HandleTurnOver(message);
        }
    }
}
