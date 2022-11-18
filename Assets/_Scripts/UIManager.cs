using Assets._Scripts.Network;
using KnjiznicarDataModel.Message;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private TMP_InputField _usernameInputField = null;
    [SerializeField] private TMP_InputField _passwordInputField = null;
    [SerializeField] private Button _loginButton = null;
    [SerializeField] private Button _registerButton = null;
    [SerializeField] private Button _logoutButton = null;
    [SerializeField] private TMP_Text _notification = null;

    public string CurrentName { get => _usernameInputField.text; }

    private void Awake()
    {
        base.Awake();
        _loginButton.onClick.AddListener(LoginRequest);
        _registerButton.onClick.AddListener(RegisterRequest);
        _logoutButton.onClick.AddListener(LogoutRequest);

        _notification.gameObject.SetActive(false);
        _logoutButton.gameObject.SetActive(false);
    }

    public void ShowNotification(string notification)
    {
        StartCoroutine(NotificationShower(notification));
    }

    private IEnumerator NotificationShower(string notification)
    {
        yield return null;
        _notification.gameObject.SetActive(true);
        _notification.text = notification;
    }

    public void LoggedIn(string username)
    {
        SceneManager.LoadScene("OverWorld");
    }

    private IEnumerator LoggInSequence(string username)
    {
        yield return null;
        _notification.gameObject.SetActive(true);
        _notification.text = "Login successful, welcome " + username;
        _usernameInputField.gameObject.SetActive(false);
        _passwordInputField.gameObject.SetActive(false);
        _loginButton.gameObject.SetActive(false);
        _registerButton.gameObject.SetActive(false);
        _logoutButton.gameObject.SetActive(true);
    }

    public void LoggedOut()
    {
        StartCoroutine(LoggOutSequence());
    }

    private IEnumerator LoggOutSequence()
    {
        yield return null;
        _notification.gameObject.SetActive(false);
        _usernameInputField.gameObject.SetActive(true);
        _passwordInputField.gameObject.SetActive(true);
        _loginButton.gameObject.SetActive(true);
        _registerButton.gameObject.SetActive(true);
        _logoutButton.gameObject.SetActive(false);
    }

    private void LoginRequest()
    {
        if (Client.Instance.LoginServer.IsConnected) SendLoginRequest();
        else Client.Instance.LoginServer.ConnectToServer(SendLoginRequest);
    }

    private void RegisterRequest()
    {
        if (Client.Instance.LoginServer.IsConnected) SendRegisterRequest();
        else Client.Instance.LoginServer.ConnectToServer(SendRegisterRequest);
    }

    private void LogoutRequest()
    {
        ClientSend.SendTCPData(new LogoutMessage(true), Client.Instance.LoginServer);
    }

    private void SendLoginRequest()
    {
        LoginMessage message = new LoginMessage()
        {
            username = _usernameInputField.text,
            passwordHash = GetHashString(_passwordInputField.text),
        };
        ClientSend.SendTCPData(message, Client.Instance.LoginServer);
    }

    private void SendRegisterRequest()
    {
        RegisterMessage message = new RegisterMessage()
        {
            username = _usernameInputField.text,
            passwordHash = GetHashString(_passwordInputField.text),
        };
        ClientSend.SendTCPData(message, Client.Instance.LoginServer);
    }

    private void ValidateText(string text)
    {
        bool exists = SpellChecker.Validate(text);
        Debug.Log(exists);
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
}
