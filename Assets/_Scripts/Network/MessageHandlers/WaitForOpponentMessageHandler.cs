using Newtonsoft.Json.Linq;
using MatchInstance;

namespace Network.MessageHandlers
{
    class WaitForOpponentMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(JObject dataJsonObject)
        {
            UIManager.Instance.ShowWaitingForOpponent();
        }
    }
}
