using KnjiznicarDataModel.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Shared;

namespace Network.MessageHandlers
{
    class MatchFoundMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(JObject dataJsonObject)
        {
            MatchFoundMessage message = JsonConvert.DeserializeObject<MatchFoundMessage>(dataJsonObject.ToString());

            GlobalGameManager.Instance.SetupInstance(message.MatchType, message.OwnData, message.EnemyName, message.EnemyHealth);
            Overworld.UIManager.Instance.LoadInstanceScene();
        }
    }
}
