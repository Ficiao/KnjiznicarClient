using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.Network
{
    public class Server
    {
        private bool _isConected = false;

        public static int DataBufferSize = 4096;
        public string Ip;
        public int Port;
        public TCP Tcp;

        public bool IsConnected { get => _isConected; set => _isConected = value; }

        public Server(string ip, int port)
        {
            Tcp = new TCP(DataBufferSize, this);
            this.Ip = ip;
            this.Port = port;
        }

        internal void ConnectToServer(Action callback)
        {
            if (_isConected) return;
            Tcp.Connect(callback);
        }

        public void Disconnect()
        {
            if (_isConected == false) return;
            _isConected = false;
            Debug.Log($"Disconnected from server.");

            Tcp.Disconnect();

            ThreadManager.ExecuteOnMainThread(() => UIManager.Instance.LoggedOut());
        }
    }
}
