using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace Overworld
{
    class AdventureLevelToggle : MonoBehaviour
    {
        private const string _BASE_TEXT = "Level ";

        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Toggle _toggle;
        private int _level;

        public void Initialize(int level, ToggleGroup toggleGroup, Action<int> toggleCallback)
        {
            _levelText.text = $"{_BASE_TEXT}{level}";
            _level = level;
            _toggle.group = toggleGroup;
            _toggle.onValueChanged.AddListener(isOn => { if (isOn) toggleCallback?.Invoke(_level); });
        }

        public void ForceSelect() => _toggle.isOn = true;
    }
}
