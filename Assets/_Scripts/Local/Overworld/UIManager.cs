using Network;
using KnjiznicarDataModel.Message;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace Overworld
{
    class UIManager : Singleton<UIManager>
    {
        [SerializeField] private Button _logoutButton = null;
        [SerializeField] private TextMeshProUGUI _pingText = null;

        private void Awake()
        {
            base.Awake();
            _logoutButton.onClick.AddListener(LogoutRequest);
        }

        private void LogoutRequest()
        {
            ClientSend.SendTCPData(new LogoutMessage(true), Client.OverworldServer);
        }

        public void LoggedOut()
        {
            StartCoroutine(LoggOutSequence());
        }

        private IEnumerator LoggOutSequence()
        {
            yield return null;
            SceneManager.LoadScene("LoginScreen");
        }

        public void SetPingTime(int time)
        {
            if (time <= 50) _pingText.color = Color.green;
            else if (time <= 75) _pingText.color = Color.yellow;
            if (time <= 35) _pingText.color = Color.green;
            _pingText.text = $"{time} ms";
        }
    }
}
