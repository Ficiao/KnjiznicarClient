using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using KnjiznicarDataModel.Message;
using Overworld;
using Shared;
using KnjiznicarDataModel.Enum;

namespace Network.MessageHandlers
{
    class PlayerCoordinatesMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(JObject dataJsonObject)
        {
            if (GlobalGameManager.Instance.Server != ServerType.Overworld) return;

            PlayerCoordinatesMessage message = JsonConvert.DeserializeObject<PlayerCoordinatesMessage>(dataJsonObject.ToString());

            GameManager.Instance.Player.NewServerState(message.ProcessedStates);
        }
    }
}
