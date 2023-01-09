using Network;
using KnjiznicarDataModel.Message;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Login
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private TMP_InputField _usernameInputField = null;
        [SerializeField] private TMP_InputField _passwordInputField = null;
        [SerializeField] private TMP_InputField _playerNameInputField = null;
        [SerializeField] private Button _loginButton = null;
        [SerializeField] private Button _registerButton = null;
        [SerializeField] private Button _selectPlayerNameButton = null;
        [SerializeField] private TMP_Text _notification = null;
        [SerializeField] private int _minUsernameLength = 0;
        [SerializeField] private int _minPasswordLength = 0;
        private string _username;

        public string CurrentName { get => _usernameInputField.text; }

        private void Awake()
        {
            base.Awake();
            _loginButton.onClick.AddListener(LoginRequest);
            _registerButton.onClick.AddListener(RegisterRequest);
            _selectPlayerNameButton.onClick.AddListener(() => SelectPlayerName(_playerNameInputField.text));

            _notification.gameObject.SetActive(false);
        }

        public void ShowNotification(string notification)
        {
            ThreadManager.ExecuteOnMainThread(() => 
            {
                EnableButtons(true);
                _notification.gameObject.SetActive(true);
                _notification.text = notification;
            });
        }

        public void LoggedIn()
        {
            SceneManager.LoadScene("OverWorld");
        }

        private void LoginRequest()
        {
            if(_usernameInputField.text.Count(c => !char.IsLetterOrDigit(c)) > 0 || _usernameInputField.text.Length < _minUsernameLength
                || _passwordInputField.text.Count(c => !char.IsLetterOrDigit(c)) > 0  || _passwordInputField.text.Length < _minPasswordLength)
            {
                ShowNotification("Please write your username and password.");
                return;
            }

            EnableButtons(false);
            if (Client.LoginServer.IsConnected) SendLoginRequest();
            else Client.LoginServer.ConnectToServer(SendLoginRequest);
        }

        private void RegisterRequest()
        {
            if (_usernameInputField.text.Count(c => !char.IsLetterOrDigit(c)) > 0 || _usernameInputField.text.Length < _minUsernameLength
                   || _passwordInputField.text.Count(c => !char.IsLetterOrDigit(c)) > 0 || _passwordInputField.text.Length < _minPasswordLength)
            {
                ShowNotification("Please write your username and password.");
                return;
            }
            EnableButtons(false);
            if (Client.LoginServer.IsConnected) SendRegisterRequest();
            else Client.LoginServer.ConnectToServer(SendRegisterRequest);
        }

        private void SendLoginRequest()
        {
            LoginMessage message = new LoginMessage()
            {
                Username = _usernameInputField.text,
                PasswordHash = GetHashString(_passwordInputField.text),
            };
            ClientSend.SendTCPData(message, Client.LoginServer);
        }

        private void SendRegisterRequest()
        {
            RegisterMessage message = new RegisterMessage()
            {
                Username = _usernameInputField.text,
                PasswordHash = GetHashString(_passwordInputField.text),
            };
            ClientSend.SendTCPData(message, Client.LoginServer);
        }

        public void ShowNameSelection(string username)
        {
            EnableButtons(true);
            _username = username;
            _usernameInputField.gameObject.SetActive(false);
            _passwordInputField.gameObject.SetActive(false);
            _loginButton.gameObject.SetActive(false);
            _registerButton.gameObject.SetActive(false);
            _notification.gameObject.SetActive(false);
            _playerNameInputField.gameObject.SetActive(true);
            _selectPlayerNameButton.gameObject.SetActive(true);
        }

        private void SelectPlayerName(string playerName)
        {
            if(playerName.Count(c => !char.IsLetterOrDigit(c)) > 0 || playerName.Length < _minUsernameLength)
            {
                ShowNotification("Please write your player name.");
                return;
            }
            if (playerName.Contains(_username))
            {
                ShowNotification("Player name must not contain username.");
                return;
            }

            EnableButtons(true);
            PlayerNameSelectionMessage message = new PlayerNameSelectionMessage()
            {
                PlayerName = playerName,
            }; 
            ClientSend.SendTCPData(message, Client.LoginServer);
        }

        private byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        private string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        private void EnableButtons(bool enabled)
        {
            _loginButton.interactable = enabled;
            _registerButton.interactable = enabled;
            _selectPlayerNameButton.interactable = enabled;
        }
    }
}