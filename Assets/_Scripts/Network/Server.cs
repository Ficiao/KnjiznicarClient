using System;
using UnityEngine;

namespace Network
{
    public class Server
    {
        private bool _isConected = false;
        private Action _logoutAction;

        public static int DataBufferSize = 4096;
        public string Ip;
        public int Port;
        public TCP Tcp;
        public UDP Udp;

        public bool IsConnected { get => _isConected; set => _isConected = value; }

        public Server(string ip, int port, Action logoutAction, bool hasUdp)
        {
            Ip = ip;
            Port = port;
            Tcp = new TCP(DataBufferSize, this);
            if (hasUdp) Udp = new UDP(this);
            _logoutAction = logoutAction;
        }

        internal void ConnectToServer(Action callback)
        {
            if (_isConected) return;
            Tcp.Connect(() => callback?.Invoke());
        }

        public void Disconnect(bool isLogout)
        {
            if (_isConected == false) return;
            _isConected = false;
            Debug.Log($"Disconnected from server.");

            Tcp?.Disconnect();
            Udp?.Disconnect();

            if (isLogout == false) return;
            try
            {
                ThreadManager.ExecuteOnMainThread(() => _logoutAction?.Invoke());
            }
            catch(Exception ex)
            {
                Debug.Log($"Message while executing async action on main thread: {ex}");
            }
        }
    }
}
