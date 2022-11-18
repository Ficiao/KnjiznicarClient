using KnjiznicarDataModel.Enum;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Assets._Scripts.Network.MessageHandlers
{
    class MessageHandler : SingletonPersistent<MessageHandler>
    {
        private Dictionary<MessageType, BaseMessageHandler> _messageHandlers = new Dictionary<MessageType, BaseMessageHandler>()
        {
            { MessageType.Connect, new ConnectedToServerMessageHandler() },
            { MessageType.LoginSuccessful, new LoginSuccessfulMessageHandler() },
            { MessageType.Logout, new LogoutMessageHandler() },
            { MessageType.PlayerCoordinates, new PlayerCoordinatesMessageHandler() },
            { MessageType.SpawnPlayer, new SpawnPlayerMessageHandler() },
        };

        public void HandleMessage(JObject dataJsonObject)
        {
            MessageType messageType = (MessageType)Int32.Parse(dataJsonObject["messageType"].ToString());
            _messageHandlers[messageType].HandleMessage(dataJsonObject);
        }
    }
}
