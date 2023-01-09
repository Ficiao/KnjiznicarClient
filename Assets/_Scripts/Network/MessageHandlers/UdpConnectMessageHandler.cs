using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Network.MessageHandlers
{
    class UdpConnectMessageHandler : BaseMessageHandler
    {
        public override void HandleMessage(JObject dataJsonObject)
        {
            Debug.Log("Connected to server UDP."); 
        }
    }
}
