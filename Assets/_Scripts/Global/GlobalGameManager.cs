using Enum;
using KnjiznicarDataModel.Enum;
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
        private List<string> _despawnCache;
        private ServerType _currentServer;
        Ping ping;
        private IEnumerator _pinger;

        public ServerType Server { get => _currentServer; }
        public string PingIp;

        private void Awake()
        {
            base.Awake();
            _spawnCache = new List<string>();
            _despawnCache = new List<string>();
            SceneManager.sceneLoaded += OnSceneLoaded;
            _currentServer = ServerType.Login;
            _pinger = PingUpdate();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            switch (scene.name)
            {
                case "LoginScreen":
                    _currentServer = ServerType.Login;
                    StopCoroutine(_pinger);
                    break;
                case "Overworld":
                    _currentServer = ServerType.Overworld;
                    OverworldLoaded();
                    StartCoroutine(_pinger);
                    break;
                case "Instance":
                    _currentServer = ServerType.Instance;
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

        public void DespawnPlayer(string playerName)
        {
            if (Overworld.GameManager.Instance == null) _despawnCache.Add(playerName);
            else Overworld.GameManager.Instance.DespawnPlayer(playerName);
        }

        private void OverworldLoaded()
        {
            _spawnCache.ForEach(s => Overworld.GameManager.Instance.SpawnPlayer(s));
            _spawnCache.Clear();      
            _despawnCache.ForEach(s => Overworld.GameManager.Instance.DespawnPlayer(s));
            _despawnCache.Clear();
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
                    case ServerType.Login:
                        break;
                    case ServerType.Overworld:
                        Overworld.UIManager.Instance.SetPingTime(ping.time);
                        break;
                    case ServerType.Instance:
                        break;
                    default:
                        break;
                }
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
