using UnityEngine;

namespace Scriptables
{
    class ScriptablesHolder : SingletonPersistent<ScriptablesHolder>
    {
        [SerializeField] private PlayerScriptable _playerScriptable;
        [SerializeField] private NpcScriptable _npcScriptable;

        public PlayerScriptable PlayerScriptable => _playerScriptable;
        public NpcScriptable NpcScriptable => _npcScriptable;

        private void Awake()
        {
            base.Awake();
            _playerScriptable.Init();
        }
    }
}
