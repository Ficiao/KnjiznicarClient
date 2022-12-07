using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using KnjiznicarDataModel.Message;
using System;
using UnityEngine;
using Overworld;
using Global;

namespace Network.MessageHandlers
{
    class PlayerCoordinatesMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(JObject dataJsonObject)
        {
            PlayerCoordinatesMessage message = JsonConvert.DeserializeObject<PlayerCoordinatesMessage>(dataJsonObject.ToString());


            foreach(PlayerCoordinatesMessage.PlayerCoordinates coordinates in message.PlayerPositions)
            {
                try
                {
                    Vector3 position = new Vector3(coordinates.position[0], coordinates.position[1], coordinates.position[2]);
                    if (coordinates.playerId == GlobalPlayerData.Instance.PlayerData.playerId)
                    {
                        GameManager.Instance.Players[coordinates.playerId].NextPosition(position, 
                            coordinates.leftRightDirection, coordinates.forwardDirection, coordinates.grounded);
                    }
                    else
                    {
                        Vector3 rotation = new Vector3(coordinates.rotation[0], coordinates.rotation[1], coordinates.rotation[2]);
                        GameManager.Instance.Players[coordinates.playerId].NextPosition(position, rotation,
                            coordinates.leftRightDirection, coordinates.forwardDirection, coordinates.grounded);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log($"Trying to update position for id {coordinates.playerId} that does not exist: {ex}");
                }
            } 
        }
    }
}
