using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using KnjiznicarDataModel.Message;
using System;
using UnityEngine;
using Overworld;
using Shared;
using System.Collections.Generic;
using KnjiznicarDataModel.Enum;

namespace Network.MessageHandlers
{
    class PlayerCoordinatesMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(JObject dataJsonObject)
        {
            if (GlobalGameManager.Instance.Server != ServerType.Overworld) return;

            PlayerCoordinatesMessage message = JsonConvert.DeserializeObject<PlayerCoordinatesMessage>(dataJsonObject.ToString());
            List<string> missingPlayers = new();

            foreach(PlayerCoordinatesMessage.PlayerCoordinates coordinates in message.PlayerPositions)
            {
                try
                {
                    Vector3 position = new Vector3(coordinates.Position[0], coordinates.Position[1], coordinates.Position[2]);
                    if (coordinates.PlayerUsername == GlobalPlayerData.PlayerData.PlayerName)
                    {
                        ((UserController)GameManager.Instance.Players[coordinates.PlayerUsername]).NextPosition(position, 
                            coordinates.LeftRightDirection, coordinates.ForwardDirection, coordinates.Grounded);
                    }
                    else
                    {
                        Vector3 rotation = new Vector3(coordinates.Rotation[0], coordinates.Rotation[1], coordinates.Rotation[2]);
                        GameManager.Instance.Players[coordinates.PlayerUsername].NextPosition(position, rotation,
                            coordinates.LeftRightDirection, coordinates.ForwardDirection, coordinates.Grounded);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log($"Trying to update position for id {coordinates.PlayerUsername} that does not exist: {ex}");
                    missingPlayers.Add(coordinates.PlayerUsername);
                }
            } 

            if(missingPlayers.Count > 0)
            {
                PlayersMissingMessage playersMissingMessage = new PlayersMissingMessage()
                {
                    MissingNames = missingPlayers,
                };

                ClientSend.SendTCPData(playersMissingMessage, Client.OverworldServer);
            }
        }
    }
}
