using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using KnjiznicarDataModel.Message;
using System;
using UnityEngine;
using Overworld;
using Global;
using System.Collections.Generic;

namespace Network.MessageHandlers
{
    class PlayerCoordinatesMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(JObject dataJsonObject)
        {
            PlayerCoordinatesMessage message = JsonConvert.DeserializeObject<PlayerCoordinatesMessage>(dataJsonObject.ToString());
            List<string> missingPlayers = new();

            foreach(PlayerCoordinatesMessage.PlayerCoordinates coordinates in message.PlayerPositions)
            {
                try
                {
                    Vector3 position = new Vector3(coordinates.position[0], coordinates.position[1], coordinates.position[2]);
                    if (coordinates.playerUsername == GlobalPlayerData.Instance.PlayerData.playerName)
                    {
                        GameManager.Instance.Players[coordinates.playerUsername].NextPosition(position, 
                            coordinates.leftRightDirection, coordinates.forwardDirection, coordinates.grounded);
                    }
                    else
                    {
                        Vector3 rotation = new Vector3(coordinates.rotation[0], coordinates.rotation[1], coordinates.rotation[2]);
                        GameManager.Instance.Players[coordinates.playerUsername].NextPosition(position, rotation,
                            coordinates.leftRightDirection, coordinates.forwardDirection, coordinates.grounded);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log($"Trying to update position for id {coordinates.playerUsername} that does not exist: {ex}");
                    missingPlayers.Add(coordinates.playerUsername);
                }
            } 

            if(missingPlayers.Count > 0)
            {
                PlayersMissingMessage playersMissingMessage = new PlayersMissingMessage()
                {
                    missingNames = missingPlayers,
                };

                ClientSend.SendTCPData(playersMissingMessage, Client.OverworldServer);
            }
        }
    }
}
