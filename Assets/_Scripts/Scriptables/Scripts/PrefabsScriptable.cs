using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "Prefabs", menuName = "Prefabs")]
    class PrefabsScriptable : ScriptableObject
    {
        private static PrefabsScriptable _instance;

        public void Init()
        {
            if (Instance == null) _instance = this;
        }

        [Serializable]
        public class Prefab
        {
            [SerializeField] public int assetId;
            [SerializeField] public string name;      
            [SerializeField] public GameObject prefab;
        }

        [SerializeField] private List<Prefab> Prefabs;
        public static PrefabsScriptable Instance { get => _instance; private set => _instance = value; }

        public GameObject GetPrefab(string key)
        {
            return Prefabs.FirstOrDefault(p => p.name.Equals(key)).prefab;
        }

        public GameObject GetPrefab(int key)
        {
            return Prefabs.FirstOrDefault(p => p.assetId == key).prefab;
        }
    }
}
