using KnjiznicarDataModel.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using UnityEngine;

namespace Network.MessageHandlers
{
    class MatchmakingCanceledMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(JObject dataJsonObject)
        {
            MatchmakingCanceledMessage message = JsonConvert.DeserializeObject<MatchmakingCanceledMessage>(dataJsonObject.ToString());
            Overworld.UIManager.Instance.HideMatchmakingView();
        }
    }
}
