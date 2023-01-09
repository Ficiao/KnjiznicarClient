using Overworld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "NPC Scriptable", menuName = "NPC Scriptable")]
    class NpcScriptable : ScriptableObject
    {
        [Serializable]
        public class NpcData
        {
            [SerializeField] public int NpcId;
            [SerializeField] public Npc npc;
            [SerializeField] public Vector3 SpawnPosition;
        }

        [SerializeField] private List<NpcData> _npcData;

        public void CreateNpcs()
        {
            _npcData.ForEach(data =>
            {
                Npc npc = Instantiate(data.npc, data.SpawnPosition, Quaternion.identity).GetComponent<Npc>();
                npc.NpcId = data.NpcId;
            });
        }
    }
}
