using Newtonsoft.Json.Linq;

namespace Network.MessageHandlers
{
    public abstract class BaseMessageHandler
    {
        public abstract void HandleMessage(JObject dataJsonObject);
    }
}
