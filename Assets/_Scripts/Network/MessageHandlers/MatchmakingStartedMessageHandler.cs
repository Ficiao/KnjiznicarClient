using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Network.MessageHandlers
{
    class MatchmakingStartedMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(JObject dataJsonObject)
        {
            Debug.Log("Matchmaking started.");
            Overworld.UIManager.Instance.ShowMatchmakingCounter();
        }
    }
}
