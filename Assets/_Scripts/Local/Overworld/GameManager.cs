using Shared;
using Scriptables;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld
{
    public class GameManager : Singleton<GameManager>
    {
        public Dictionary<string, PlayerController> Players;
        public UserController Player;

        private PlayerScriptable _playerScriptable;
        private NpcScriptable _npcScriptable;

        public void Awake()
        {
            base.Awake();
            Players = new Dictionary<string, PlayerController>();
            _playerScriptable = ScriptablesHolder.Instance.PlayerScriptable;
            _npcScriptable = ScriptablesHolder.Instance.NpcScriptable;

            _npcScriptable.CreateNpcs();
        }

        public void SpawnPlayer(string playerName)
        {
            if(playerName == GlobalPlayerData.PlayerData.PlayerName)
            {
                Player = Instantiate(_playerScriptable.GetPrefab(AssetType.OverworldLocalPlayer)).GetComponent<UserController>();
                PlayerController playerManager = Player.GetComponent<PlayerController>();
                playerManager.PlayerName = playerName;
                lock(Players)
                {
                    Players.Add(playerName, Player);
                }
            }
            else
            {
                PlayerController playerManager = Instantiate(_playerScriptable.GetPrefab(AssetType.OverworldOtherPlayer)).GetComponent<PlayerController>();
                playerManager.PlayerName = playerName;
                lock (Players)
                {
                    Players.Add(playerName, playerManager);
                }
            }
        }

        public void DespawnPlayer(string playerName)
        {
            lock (Players)
            {
                if (Players.ContainsKey(playerName))
                {
                    Destroy(Players[playerName].gameObject);
                    Players.Remove(playerName);
                }
            }
        }
    }
}