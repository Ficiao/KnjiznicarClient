using KnjiznicarDataModel.Enum;
using KnjiznicarDataModel.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.IO;
using UnityEngine;

namespace Assets._Scripts.Network
{
    class ClientSend
    {
        public static void SendTCPData(BaseMessage message, Server server)
        {
            MemoryStream ms = new MemoryStream();
            using (BsonWriter writer = new BsonWriter(ms))
            {
                JsonSerializer serializer = new JsonSerializer();
                switch (message.messageType)
                {
                    case MessageType.Connect:
                        serializer.Serialize(writer, (LoginMessage)message);
                        break;
                    case MessageType.Register:
                        serializer.Serialize(writer, (RegisterMessage)message);
                        break;
                    case MessageType.Login:
                        serializer.Serialize(writer, (LoginMessage)message);
                        break;
                    case MessageType.Logout:
                        serializer.Serialize(writer, (LogoutMessage)message);
                        break;
                    case MessageType.PlayerInput:
                        serializer.Serialize(writer, (PlayerInputMessage)message);
                        break;
                    default:
                        Debug.LogError($"Sending message with non existing type {message.messageType}.");
                        break;
                }
            }

            string json = Convert.ToBase64String(ms.ToArray());

            byte[] sendData = Convert.FromBase64String(json);
            server.Tcp.SendData(sendData);
        }
    }
}
