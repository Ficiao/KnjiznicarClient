using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets._Scripts.Scriptables
{
    [CreateAssetMenu(fileName = "Prefabs", menuName = "Prefabs")]
    class PrefabsScriptable : ScriptableObject
    {
        [Serializable]
        public class Prefab
        {
            [SerializeField]
            public string name;
            [SerializeField]
            public GameObject prefab;
        }

        private static PrefabsScriptable _instance;

        [SerializeField]
        private List<Prefab> Prefabs;
        public static PrefabsScriptable Instance { get => _instance; private set => _instance = value; }

        public GameObject GetPrefab(string key)
        {
            return Prefabs.FirstOrDefault(p => p.name.Equals(key)).prefab;
        }

        private void OnEnable()
        {
            if (Instance == null) _instance = this;
            else Destroy(this);
        }
    }
}
