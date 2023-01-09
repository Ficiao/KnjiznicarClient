using Network;
using KnjiznicarDataModel.Message;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Shared;
using KnjiznicarDataModel.Enum;
using KnjiznicarDataModel;
using System;

namespace Overworld
{
    class NetcodeMenuView : MonoBehaviour
    {
        [SerializeField] private GameObject _netcodeMenu;
        [SerializeField] private Button _netcodeMenuButton;
        [SerializeField] private TMP_InputField _lagInput;
        [SerializeField] private Toggle _clientPreditionToggle;
        [SerializeField] private Toggle _entityInterpolationToggle;
        [SerializeField] private Button _backButton;

        private void Awake()
        {
            _netcodeMenuButton.onClick.AddListener(ShowNetcodeMenu);
            _backButton.onClick.AddListener(HideNetcodeMenu);
            _lagInput.onValueChanged.AddListener(text => LagInputChanged(Int32.Parse(text)));
            _clientPreditionToggle.onValueChanged.AddListener(isOn => ClientPredictionToggleChanged(isOn));
            _entityInterpolationToggle.onValueChanged.AddListener(isOn => EntityInterpolationToggleChange(isOn));
        }

        private void ShowNetcodeMenu()
        {
            _netcodeMenu.SetActive(true);
            GameManager.Instance.Player.Enabled = false;
            CameraController.Instance.DisableCameraMovement = true;
        }

        private void HideNetcodeMenu()
        {
            _netcodeMenu.SetActive(true);
            GameManager.Instance.Player.Enabled = true;
            CameraController.Instance.DisableCameraMovement = false;
        }

        private void LagInputChanged(int amount)
        {

        }

        private void ClientPredictionToggleChanged(bool isOn)
        {

        }

        private void EntityInterpolationToggleChange(bool isOn)
        {

        }
    }
}
