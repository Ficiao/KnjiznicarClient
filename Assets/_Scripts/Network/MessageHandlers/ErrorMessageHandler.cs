using KnjiznicarDataModel.Enum;
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

            switch (message.Error)
            {
                case ErrorType.PlayerNameInvalid:
                    Login.UIManager.Instance.ShowNotification("Selecting player name unssucessfull, invalid player name.");
                    break;
                case ErrorType.PlayerCredentialsInvalid:
                    Login.UIManager.Instance.ShowNotification("Login unssucessful, wrong credentials.");
                    break;
                case ErrorType.RegisterCredentialsInvalid:
                    Login.UIManager.Instance.ShowNotification("Register unssucessful, invalid credentials.");
                    break;
                case ErrorType.MatchmakingUnsucessful:
                    //Overworld.UIManager.Instance.ShowNotification("Register unssucessful, invalid credentials.");
                    Overworld.UIManager.Instance.MatchMakingUnsucessful();
                    break;
                case ErrorType.IllegalWordUsed:
                    MatchInstance.UIManager.Instance.IllegealWordUsed();
                    break;
                default:
                    break;
            }
        }
    }
}
