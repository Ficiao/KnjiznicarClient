using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Shared;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "Player Scriptable", menuName = "Player Scriptable")]
    class PlayerScriptable : ScriptableObject
    {
        private static PlayerScriptable _instance;

        public void Init()
        {
            if (Instance == null) _instance = this;
        }

        [Serializable]
        public class Prefab
        {
            [SerializeField] public AssetType AssetId;   
            [SerializeField] public GameObject AssetPrefab;
        }

        [SerializeField] private List<Prefab> _prefabs;
        public static PlayerScriptable Instance { get => _instance; private set => _instance = value; }

        public GameObject GetPrefab(AssetType asset)
        {
            return _prefabs.FirstOrDefault(p => p.AssetId == asset).AssetPrefab;
        }
    }
}
