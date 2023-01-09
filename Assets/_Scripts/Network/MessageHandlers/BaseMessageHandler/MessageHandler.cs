using KnjiznicarDataModel.Enum;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Network.MessageHandlers
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
            { MessageType.Error, new ErrorMessageHandler() },
            { MessageType.UdpConnect, new UdpConnectMessageHandler() },
            { MessageType.DespawnPlayer, new DespawnPlayerMessageHandler() },
            { MessageType.SpawnPlayers, new SpawnPlayersMessageHandler() },
            { MessageType.HeartBeat, new HeartBeatMessageHandler() },
            { MessageType.ChatMessage, new PlayerChatMessageHandler() },
            { MessageType.SearchingForMatch, new MatchmakingStartedMessageHandler() },
            { MessageType.MatchFound, new MatchFoundMessageHandler() },
            { MessageType.MatchmakingCanceled, new MatchmakingCanceledMessageHandler() },
            { MessageType.MatchEnd, new MatchEndMessageHandler() },
            { MessageType.TurnOver, new TurnOverMessageHandler() },
            { MessageType.WaitForOpponent, new WaitForOpponentMessageHandler() },
            { MessageType.PvpLeaderboardUpdate, new PvpLeaderboardUpdateMessageHandler() },
            { MessageType.AdventureLeaderboardUpdate, new AdventureLeaderboardUpdateMessageHandler() },
        };

        public void HandleMessage(JObject dataJsonObject)
        {
            try
            {
                MessageType messageType = (MessageType)Int32.Parse(dataJsonObject["MessageType"].ToString());
                _messageHandlers[messageType].HandleMessage(dataJsonObject);
            }
            catch(Exception ex)
            {
                Debug.Log($"Error processing message: {dataJsonObject.ToString()}\n{ex}");
            }
        }
    }
}
