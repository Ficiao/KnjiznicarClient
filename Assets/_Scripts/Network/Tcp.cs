﻿using Assets._Scripts.Network.Message;
using Assets._Scripts.Network.MessageHandlers;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

namespace Assets._Scripts.Network
{
    public class TCP
    {
        private NetworkStream _stream;
        private byte[] _recieveBuffer;
        private BaseMessage recievedMessage;
        private Server _server;
        private Action UICallback;

        public TcpClient Socket;
        public static int DataBufferSize = 4096;

        public TCP(int dataBufferSize, Server server)
        {
            DataBufferSize = dataBufferSize;
            _server = server;
        }

        public void Connect(Action callback)
        {
            UICallback = callback;
            Socket = new TcpClient
            {
                ReceiveBufferSize = DataBufferSize,
                SendBufferSize = DataBufferSize
            };

            _recieveBuffer = new byte[DataBufferSize];
            Socket.BeginConnect(_server.Ip, _server.Port, ConnectCallback, Socket);
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            if (Socket.Connected == false)
            {
                Debug.Log("Server is offline.");
                return;
            }

            Socket.EndConnect(ar);
            _stream = Socket.GetStream();
            _stream.BeginRead(_recieveBuffer, 0, DataBufferSize, RecieveDataCallback, null);
            _server.IsConnected = true;
            UICallback();
        }

        private void RecieveDataCallback(IAsyncResult ar)
        {
            try
            {
                int byteLength = _stream.EndRead(ar);
                if (byteLength <= 0)
                {
                    Client.Instance.Disconnect();
                    return;
                }

                byte[] data = new byte[byteLength];
                Array.Copy(_recieveBuffer, data, byteLength);

                _stream.BeginRead(_recieveBuffer, 0, DataBufferSize, RecieveDataCallback, null);
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
                Debug.Log(ex.Message);
                Client.Instance.Disconnect();
            }
        }

        public void SendData(byte[] sendData)
        {
            try
            {
                if (Socket != null)
                {
                    _stream.BeginWrite(sendData, 0, sendData.Length, null, null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error whiel sending data to server via TCP: {ex}");
                Client.Instance.Disconnect();
            }
        }

        public void Disconnect()
        {
            Socket.Close();
            _stream = null;
            _recieveBuffer = null;
            Socket = null;
        }

        //private bool HandleData(byte[] data)
        //{
        //    int messageLength = 0;


        //}
    }
}
