using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Overworld
{
    class NetcodeMenuView : Singleton<NetcodeMenuView>
    {
        [SerializeField] private GameObject _netcodeMenu;
        [SerializeField] private Button _netcodeMenuButton;
        [SerializeField] private TMP_InputField _lagInput;
        [SerializeField] private Toggle _clientPreditionToggle;
        [SerializeField] private Toggle _entityInterpolationToggle;
        [SerializeField] private Button _backButton;

        public int MsDelay;

        private void Awake()
        {
            base.Awake();
            gameObject.SetActive(false);
            MsDelay = 0;
            _netcodeMenuButton.onClick.AddListener(ShowNetcodeMenu);
            _backButton.onClick.AddListener(HideNetcodeMenu);
            _lagInput.onValueChanged.AddListener(text => LagInputChanged(text));
            _clientPreditionToggle.onValueChanged.AddListener(isOn => ClientPredictionToggleChanged(isOn));
            _entityInterpolationToggle.onValueChanged.AddListener(isOn => EntityInterpolationToggleChange(isOn));
        }

        private void LagInputChanged(string amount)
        {
            try
            {
                MsDelay = Int32.Parse(amount);
            }
            catch { }
        }

        private void ShowNetcodeMenu()
        {
            _netcodeMenu.SetActive(true);
            GameManager.Instance.Player.Enabled = false;
            CameraController.Instance.DisableCameraMovement = true;
        }

        private void HideNetcodeMenu()
        {
            _netcodeMenu.SetActive(false);
            GameManager.Instance.Player.Enabled = true;
            CameraController.Instance.DisableCameraMovement = false;
        }


        private void ClientPredictionToggleChanged(bool isOn) => GameManager.Instance.Player.AdvancedMovementEnabled = isOn;

        private void EntityInterpolationToggleChange(bool isOn)
        {
            foreach (PlayerController player in GameManager.Instance.Players.Values)
            {
                if (player != GameManager.Instance.Player) player.AdvancedMovementEnabled = isOn;
            }
        }
    }
}
