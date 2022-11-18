using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using KnjiznicarDataModel.Message;
using System;
using UnityEngine;

namespace Assets._Scripts.Network.MessageHandlers
{
    class PlayerCoordinatesMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(JObject dataJsonObject)
        {
            PlayerCoordinatesMessage message = JsonConvert.DeserializeObject<PlayerCoordinatesMessage>(dataJsonObject.ToString());

            try
            {
                float[] coordinateArray = message.position;
                GameManager.Instance.Players[message.playerId].Move(new Vector3(coordinateArray[0], coordinateArray[1], coordinateArray[2]));
            }
            catch(Exception ex)
            {
                Debug.Log("Trying to move player with non existent id.");
            }
        }
    }
}
