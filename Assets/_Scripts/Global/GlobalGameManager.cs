using Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Global
{
    class GlobalGameManager : SingletonPersistent<GlobalGameManager>
    {
        private List<string> _spawnCache;
        private Server _currentServer;
        Ping ping;
        private IEnumerator _pinger;

        public Server Server { get => _currentServer; }
        public string PingIp;

        private void Awake()
        {
            base.Awake();
            _spawnCache = new List<string>();
            SceneManager.sceneLoaded += OnSceneLoaded;
            _currentServer = Server.Login;
            _pinger = PingUpdate();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            switch (scene.name)
            {
                case "LoginScreen":
                    _currentServer = Server.Login;
                    StopCoroutine(_pinger);
                    break;
                case "Overworld":
                    _currentServer = Server.Overworld;
                    OverworldLoaded();
                    StartCoroutine(_pinger);
                    break;
                case "Instance":
                    _currentServer = Server.Instance;
                    InstanceLoaded();
                    break;
                default:
                    break;
            }
        }

        public void SpawnPlayer(string playerName)
        {
            if (Overworld.GameManager.Instance == null) _spawnCache.Add(playerName);
            else Overworld.GameManager.Instance.SpawnPlayer(playerName);
        }

        private void OverworldLoaded()
        {
            _spawnCache.ForEach(s => Overworld.GameManager.Instance.SpawnPlayer(s));
            _spawnCache.Clear();
        }

        private void InstanceLoaded()
        {

        }

        IEnumerator PingUpdate()
        {
            while (true)
            {
                ping = new Ping("159.223.16.231");
                while (!ping.isDone) yield return null;
                switch (_currentServer)
                {
                    case Server.Login:
                        break;
                    case Server.Overworld:
                        Overworld.UIManager.Instance.SetPingTime(ping.time);
                        break;
                    case Server.Instance:
                        break;
                    default:
                        break;
                }
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
