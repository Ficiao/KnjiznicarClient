using KnjiznicarDataModel;
using KnjiznicarDataModel.Enum;
using Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shared
{
    class GlobalGameManager : SingletonPersistent<GlobalGameManager>
    {
        private List<string> _overworldSpawnCache;
        private List<string> _overworldDespawnCache;
        private LeaderboardData _pvpLeaderboardCache;
        private LeaderboardData _adventureLeaderboardCache;
        private (MatchType, PlayerMatchData, string, int) _instanceSetupCache;
        private ServerType _currentServer;
        Ping ping;
        private IEnumerator _pinger;

        public ServerType Server { get => _currentServer; }
        public string PingIp;
        public bool IsWrongVersion;

        private void Awake()
        {
            bool instanceExists = Instance != null && Instance != this;
            base.Awake();
            if (instanceExists) return;
            _overworldSpawnCache = new List<string>();
            _overworldDespawnCache = new List<string>();
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
                    LoginScreenLoaded();
                    _pinger = PingUpdate();
                    break;
                case "Overworld":
                    _currentServer = ServerType.Overworld;
                    OverworldLoaded();
                    if(_pinger != null)
                    {
                        StopCoroutine(_pinger);
                        _pinger = PingUpdate();
                    }
                    StartCoroutine(_pinger);
                    break;
                case "Instance":
                    if (_pinger != null)
                    {
                        StopCoroutine(_pinger);
                        _pinger = PingUpdate();
                    }
                    StartCoroutine(_pinger);
                    _currentServer = ServerType.Instance;
                    InstanceLoaded();
                    break;
                default:
                    break;
            }
        }

        public void SpawnPlayer(string playerName)
        {
            if (Overworld.GameManager.Instance == null) lock (_overworldSpawnCache) _overworldSpawnCache.Add(playerName);
            else
            {
                lock (Overworld.GameManager.Instance)
                {
                    if (Overworld.GameManager.Instance.Players.ContainsKey(playerName)) return;
                    Overworld.GameManager.Instance.SpawnPlayer(playerName);
                }
            }
        }

        public void UpdatePvpLeaderboard(LeaderboardData data)
        {
            if (Overworld.UIManager.Instance == null) _pvpLeaderboardCache = data;
            else Overworld.UIManager.Instance.UpdatePvpLeaderboard(data);
        }

        public void UpdateAdventureLeaderboard(LeaderboardData data)
        {
            if (Overworld.UIManager.Instance == null) _adventureLeaderboardCache = data;
            else Overworld.UIManager.Instance.UpdateAdventureLeaderboard(data);
        }

        public void DespawnPlayer(string playerName)
        {
            if (Overworld.GameManager.Instance == null) lock (_overworldDespawnCache) _overworldDespawnCache.Add(playerName);
            else
            {
                lock (Overworld.GameManager.Instance)
                {
                    if (!Overworld.GameManager.Instance.Players.ContainsKey(playerName)) return;
                    Overworld.GameManager.Instance.DespawnPlayer(playerName);
                }
            }
        }

        private void LoginScreenLoaded()
        {
            if (IsWrongVersion)
            {
                IsWrongVersion = false;
                Login.UIManager.Instance.ShowNotification("Wrong game version. Please return to launcher and download latest version.");
            }
        }

            private void OverworldLoaded()
        {
            _overworldSpawnCache.ForEach(s => 
            {
                lock (Overworld.GameManager.Instance)
                {
                    if(!Overworld.GameManager.Instance.Players.ContainsKey(s)) Overworld.GameManager.Instance.SpawnPlayer(s);
                };
            });
            _overworldSpawnCache.Clear();      
            _overworldDespawnCache.ForEach(s =>
            {
                lock (Overworld.GameManager.Instance)
                {
                    if (Overworld.GameManager.Instance.Players.ContainsKey(s)) Overworld.GameManager.Instance.DespawnPlayer(s);
                }
            });
            _overworldDespawnCache.Clear();

            if(_pvpLeaderboardCache != null)
            {
                Overworld.UIManager.Instance.UpdatePvpLeaderboard(_pvpLeaderboardCache);
                _pvpLeaderboardCache = null;
            }

            if(_adventureLeaderboardCache != null)
            {
                Overworld.UIManager.Instance.UpdateAdventureLeaderboard(_adventureLeaderboardCache);
                _adventureLeaderboardCache = null;
            }

            Overworld.UIManager.Instance.ShowProfileData(GlobalPlayerData.PlayerData.PlayerName,
                GlobalPlayerData.PlayerData.AdventureLevel, GlobalPlayerData.PlayerData.PvpPoints);
        }

        private void InstanceLoaded()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            MatchInstance.GameManager.Instance.SetupScene(_instanceSetupCache.Item1, _instanceSetupCache.Item2, _instanceSetupCache.Item3, _instanceSetupCache.Item4);
            _instanceSetupCache = ((MatchType)(-1), null, null, 0);
        }

        public void SetupInstance(MatchType matchType, PlayerMatchData playerData, string enemyName, int enemyHealth)
        {
            if (MatchInstance.GameManager.Instance == null) _instanceSetupCache = (matchType, playerData, enemyName, enemyHealth);
            else MatchInstance.GameManager.Instance.SetupScene(matchType, playerData, enemyName, enemyHealth);
        }

        IEnumerator PingUpdate()
        {
            while (true)
            {
                ping = new Ping(Client.Instance.ServerIp);
                while (!ping.isDone) yield return null;
                switch (_currentServer)
                {
                    case ServerType.Login:
                        break;
                    case ServerType.Overworld:
                        Overworld.UIManager.Instance.SetPingTime(ping.time);
                        break;
                    case ServerType.Instance:
                        MatchInstance.UIManager.Instance.SetPingTime(ping.time);
                        break;
                    default:
                        break;
                }
                yield return new WaitForSeconds(1.5f);
            }
        }
    }
}
