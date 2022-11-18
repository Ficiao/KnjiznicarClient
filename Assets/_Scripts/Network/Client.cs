using System;
using UnityEngine;

namespace Assets._Scripts.Network
{
    public class Client : SingletonPersistent<Client>
    {
        public Server LoginServer;
        public Server OverworldServer;
        public Server InstanceServer;

        private void Start()
        {
            LoginServer = new Server("127.0.0.1", 26950);
        }

        private void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            LoginServer.Disconnect();
        }

        public void ConnectToWorldServer(string ip, int port)
        {
            OverworldServer = new Server(ip, port);
            OverworldServer.ConnectToServer(() => ConnectedToServer("Connected to overworld server."));
        }

        public void ConnectToInstanceServer(string ip, int port)
        {
            InstanceServer = new Server(ip, port);
        }

        private void ConnectedToServer(string message)
        {
            Debug.Log(message);
        }

        public void Disconnect()
        {
            LoginServer.Disconnect();
        }
    }
}
