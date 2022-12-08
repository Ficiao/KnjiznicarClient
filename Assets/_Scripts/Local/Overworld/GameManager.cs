using Global;
using Scriptables;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private Camera _camera;

        public Dictionary<string, PlayerController> Players;
        public UserController Player;

        public void Awake()
        {
            base.Awake();
            Players = new Dictionary<string, PlayerController>();
        }
        public void SpawnPlayer(string playerName)
        {
            if(playerName == GlobalPlayerData.Instance.PlayerData.playerName)
            {
                Player = Instantiate(PrefabsScriptable.Instance.GetPrefab("Player")).GetComponent<UserController>();
                PlayerController playerManager = Player.GetComponent<PlayerController>();
                playerManager.PlayerName = playerName;
                Players.Add(playerName, playerManager);
                _camera.transform.parent = playerManager.transform;
            }
            else
            {
                PlayerController playerManager = Instantiate(PrefabsScriptable.Instance.GetPrefab("OtherPlayer")).GetComponent<PlayerController>();
                playerManager.PlayerName = playerName;
                Players.Add(playerName, playerManager);
            }
        }
    }
}