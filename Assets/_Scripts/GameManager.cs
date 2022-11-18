using Assets._Scripts.Scriptables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Scripts
{
    public class GameManager : SingletonPersistent<GameManager>
    {
        public Dictionary<int, PlayerManager> Players = new Dictionary<int, PlayerManager>();
        public PlayerController Player;

        public void SpawnPlayer(int id, string username)
        {
            if(id == GlobalPlayerData.Instance.playerId)
            {
                Player = Instantiate(PrefabsScriptable.Instance.GetPrefab("Player")).GetComponent<PlayerController>();
                PlayerManager playerManager = Player.GetComponent<PlayerManager>();
                playerManager.Username = username;
                Players.Add(id, playerManager);
            }
            else
            {
                PlayerManager playerManager = Instantiate(PrefabsScriptable.Instance.GetPrefab("OtherPlayer")).GetComponent<PlayerManager>();
                playerManager.Username = username;
                Players.Add(id, playerManager);
            }
        }
    }
}