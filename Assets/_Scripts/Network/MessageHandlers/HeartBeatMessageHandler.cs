using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using KnjiznicarDataModel.Message;

namespace Network.MessageHandlers
{
    class HeartBeatMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(JObject dataJsonObject)
        {
            HeartBeatMessage message = JsonConvert.DeserializeObject<HeartBeatMessage>(dataJsonObject.ToString());
        }
    }
}
