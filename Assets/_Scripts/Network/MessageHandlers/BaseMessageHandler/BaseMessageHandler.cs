using Newtonsoft.Json.Linq;

namespace Assets._Scripts.Network.MessageHandlers
{
    public abstract class BaseMessageHandler
    {
        public abstract void HandleMessage(JObject dataJsonObject);
    }
}
