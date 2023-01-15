using KnjiznicarDataModel.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MatchInstance;

namespace Network.MessageHandlers
{
    class MatchEndMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(JObject dataJsonObject)
        {
            MatchEndMessage message = JsonConvert.DeserializeObject<MatchEndMessage>(dataJsonObject.ToString());

            UIManager.Instance.ShowMatchEndView(message);
        }
    }
}
