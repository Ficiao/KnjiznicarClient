using UnityEngine;

namespace Scriptables
{
    class ScriptablesHolder : SingletonPersistent<ScriptablesHolder>
    {
        [SerializeField] private PrefabsScriptable _prefabsScriptable;

        private void Awake()
        {
            base.Awake();
            _prefabsScriptable.Init();
        }
    }
}
