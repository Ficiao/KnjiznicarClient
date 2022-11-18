using Assets._Scripts.Network.Enum;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Scripts.Network.Message
{
    class SpawnPlayerMessage : BaseMessage
    {
        public int playerId;
        public string playerUsername;
        public float[] spawnPosition = new float[3];

        public SpawnPlayerMessage() : base(MessageType.SpawnPlayer)
        {
        }
    }
}
