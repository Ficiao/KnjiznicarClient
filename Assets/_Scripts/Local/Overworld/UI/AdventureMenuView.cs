using UnityEngine;
using UnityEngine.UI;

namespace Overworld
{
    class AdventureMenuView : MonoBehaviour
    {
        [SerializeField] private ToggleGroup _toggleContainer;
        [SerializeField] private AdventureLevelToggle _togglePrefab;
        private AdventureLevelToggle _defaultToggle;

        public int SelectedLevel;

        private void SelectedLevelChanged(int level) => SelectedLevel = level;

        public void ResetToDefault() => _defaultToggle.ForceSelect();

        public void Initialize(int maxLevel)
        {
            AdventureLevelToggle toggle = Instantiate(_togglePrefab, _toggleContainer.transform);
            toggle.Initialize(1, _toggleContainer, SelectedLevelChanged);
            _defaultToggle = toggle;

            for (int i = 2; i <= maxLevel; i++)
            {
                toggle = Instantiate(_togglePrefab, _toggleContainer.transform);
                toggle.Initialize(i, _toggleContainer, SelectedLevelChanged);
            }
        }        
    }
}
