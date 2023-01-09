using KnjiznicarDataModel.Message;
using Network.MessageHandlers;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using System.Net;

namespace Network
{
    public class UDP
    {
        private IPEndPoint _endPoint;
        private Server _server;
        private NetworkStream _stream;
        private byte[] _recieveBuffer;

        public UdpClient Socket;

        public UDP(Server server)
        {
            _server = server;
            _endPoint = new IPEndPoint(IPAddress.Parse(_server.Ip), _server.Port);
        }

        public void Connect()
        {
            Socket = new UdpClient();
            Socket.Connect(_endPoint);
            _recieveBuffer = new byte[1];
            Socket.BeginReceive(RecieveCallback, null);

            ClientSend.SendUDPData(new UdpConnectMessage()
            {
                SessionId = Client.SessionId,
            }, 
            _server);   
        }

        public void SendData(byte[] sendData)
        {
            try
            {
                if (Socket != null)
                {
                    Socket.BeginSend(sendData, sendData.Length, null, null);
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"Error ending data to server via UDP: {ex}");
            }
        }

        private void RecieveCallback(IAsyncResult _result)
        {
            try
            {
                byte[] data = Socket.EndReceive(_result, ref _endPoint);
                Socket.BeginReceive(RecieveCallback, null);

                if (data.Length < 4)
                {
                    _server.Disconnect(true);
                    return;
                }

                if (data.Length > _recieveBuffer.Length) _recieveBuffer = new byte[data.Length];
                Array.Copy(data, _recieveBuffer, data.Length);

                ThreadManager.ExecuteOnMainThread(() =>
                {
                    MemoryStream ms = new MemoryStream(data);
                    JObject dataJsonObject;
                    using (BsonReader reader = new BsonReader(ms))
                    {
                        dataJsonObject = (JObject)JToken.ReadFrom(reader);
                        MessageHandler.Instance.HandleMessage(dataJsonObject);
                    }
                });
            }
            catch (Exception ex)
            {
                Disconnect();
            }
        }

        public void Disconnect()
        {
            _server.Disconnect(true);

            _endPoint = null;
            Socket = null;
        }
    }
}
