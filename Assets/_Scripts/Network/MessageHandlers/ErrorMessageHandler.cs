using KnjiznicarDataModel.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Network.MessageHandlers
{
    class ErrorMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(JObject dataJsonObject)
        {
            ErrorMessage message = JsonConvert.DeserializeObject<ErrorMessage>(dataJsonObject.ToString());

            switch (message.error)
            {
                case KnjiznicarDataModel.Enum.ErrorType.PlayerNameInvalid:
                    Login.UIManager.Instance.ShowNotification("Selecting player name unssucessfull, invalid player name.");
                    break;
                case KnjiznicarDataModel.Enum.ErrorType.PlayerCredentialsInvalid:
                    Login.UIManager.Instance.ShowNotification("Login unssucessful, wrong credentials.");
                    break;
                case KnjiznicarDataModel.Enum.ErrorType.RegisterCredentialsInvalid:
                    Login.UIManager.Instance.ShowNotification("Register unssucessful, invalid credentials.");
                    break;
                default:
                    break;
            }
        }
    }
}
