using KnjiznicarDataModel.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.IO;

namespace Network
{
    class ClientSend
    {
        public static void SendTCPData<T>(T message, Server server) where T : BaseMessage
        {
            MemoryStream ms = new MemoryStream();
            using (BsonWriter writer = new BsonWriter(ms))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, (T)message);                
            }

            string json = Convert.ToBase64String(ms.ToArray());

            byte[] sendData = Convert.FromBase64String(json);
            server.Tcp.SendData(sendData);
        }

        public static void SendUDPData<T>(T message, Server server) where T : BaseMessage
        {
            MemoryStream ms = new MemoryStream();
            using (BsonWriter writer = new BsonWriter(ms))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, (T)message);
            }

            string json = Convert.ToBase64String(ms.ToArray());

            byte[] sendData = Convert.FromBase64String(json);
            server.Udp.SendData(sendData);
        }
    }
}
