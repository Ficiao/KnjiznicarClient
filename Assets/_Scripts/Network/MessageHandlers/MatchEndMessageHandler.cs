using KnjiznicarDataModel.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using MatchInstance;
using KnjiznicarDataModel.Enum;

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
