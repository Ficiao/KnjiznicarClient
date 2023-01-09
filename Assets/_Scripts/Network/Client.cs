using Shared;
using UnityEngine;

namespace Network
{
    public class Client : SingletonPersistent<Client>
    {
        [SerializeField] private string _loginIp;

        public static string SessionId;
        public static Server LoginServer { get; private set; }
        public static Server OverworldServer{ get; private set; }
        public static Server InstanceServer { get; private set; }

        private void Start()
        {
            LoginServer = new Server(_loginIp, 26950, null, false);
        }

        private void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            LoginServer.Disconnect(true);
        }

        public void ConnectToWorldServer(string ip, int port)
        {
            OverworldServer = new Server(ip, port, () => Overworld.UIManager.Instance.LoggedOut(), true);
            OverworldServer.ConnectToServer(() => ConnectedToServer("Connected to overworld server."));
            GlobalGameManager.Instance.PingIp = ip;
        }

        public void ConnectToInstanceServer(string ip, int port)
        {
            InstanceServer = new Server(ip, port, null, false);
        }

        private void ConnectedToServer(string message)
        {
            Debug.Log(message);
        }

        public void DisconnectAll(bool isLogout)
        {
            LoginServer?.Disconnect(isLogout);
            OverworldServer?.Disconnect(isLogout);
            InstanceServer?.Disconnect(isLogout);
        }
    }
}
