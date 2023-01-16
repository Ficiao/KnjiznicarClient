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

namespace Overworld
{
    class UIManager : Singleton<UIManager>
    {
        private const string _BASE_LOOKING_FOR_MATCH_TEXT = "Looking for match";
        private const string _BASE_ADVENTURE_LEVEL_TEXT = "Adventure level: ";
        private const string _BASE_PVP_RATING_TEXT = "PvP rating: ";

        [SerializeField] private Button _logoutButton;
        [SerializeField] private TextMeshProUGUI _pingText;
        [SerializeField] private TMP_InputField _chatInput;
        [SerializeField] private AdventureMenuView _adventureMenu;
        [SerializeField] private GameObject _interactTooltip;
        [SerializeField] private Button _returnFromAdventureMenuButton;
        [SerializeField] private Button _selectAdventureLevelButton;
        [SerializeField] private GameObject _lookingForMatchView;
        [SerializeField] private TextMeshProUGUI _lookingForMatchText;
        [SerializeField] private Button _cancelLookingForMatchButton;
        [SerializeField] private TextMeshPro _pvpLeaderBoard;
        [SerializeField] private TextMeshPro _adventureLeaderBoard;
        [SerializeField] private TextMeshProUGUI _profileName;
        [SerializeField] private TextMeshProUGUI _profileAdventureLevel;
        [SerializeField] private TextMeshProUGUI _provilePvpRating;
        private IEnumerator _lookingForMatchAnimation;
        private int? _npcId;
        private NetcodeMenuView _netcodeMenu;

        private void Awake()
        {
            base.Awake();
            _logoutButton.onClick.AddListener(LogoutRequest);
            _returnFromAdventureMenuButton.onClick.AddListener(HideAdventureMenu);
            _selectAdventureLevelButton.onClick.AddListener(RequestAdventureLevel);
            _cancelLookingForMatchButton.onClick.AddListener(CancelMatchmaking);
            _chatInput.onSubmit.AddListener(text => SendChatMessage(text));
            _chatInput.onSelect.AddListener(text => GameManager.Instance.Player.Enabled = false);
            _chatInput.onDeselect.AddListener(text => GameManager.Instance.Player.Enabled = true);
            _adventureMenu.Initialize(GlobalPlayerData.PlayerData.AdventureLevel + 1);
        }

        private void Start()
        {
            _netcodeMenu = NetcodeMenuView.Instance;
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

        private void SendChatMessage(string text)
        {
            if (text != null && text.Length <= 0) return;
            PlayerChatMessage message = new PlayerChatMessage()
            {
                Message = text,
            };
            ClientSend.SendTCPData(message, Client.OverworldServer);
            _chatInput.text = null;
        }

        public void SetPingTime(int time)
        {
            time += _netcodeMenu.MsDelay;
            if (time <= 50) _pingText.color = Color.green;
            else if (time <= 100) _pingText.color = Color.yellow;
            else _pingText.color = Color.red;
            _pingText.text = $"{time} ms";
        }

        public void ShowInteractTooltip()
        {
            _interactTooltip.SetActive(true);
        }

        public void HideInteractTooltip()
        {
            _interactTooltip.SetActive(false);
        }

        public void ShowAdventureMenu(int npcId)
        {
            _npcId = npcId;
            _adventureMenu.ResetToDefault();
            _adventureMenu.gameObject.SetActive(true);
            GameManager.Instance.Player.Enabled = false;
            CameraController.Instance.DisableCameraMovement = true;
        }

        private void HideAdventureMenu()
        {
            _npcId = null;
            _adventureMenu.gameObject.SetActive(false);
            GameManager.Instance.Player.Enabled = true;
            CameraController.Instance.DisableCameraMovement = false;
        }

        private void RequestAdventureLevel()
        {
            _selectAdventureLevelButton.interactable = false;
            MatchmakingRequestMessage message = new MatchmakingRequestMessage()
            {
                MatchType = MatchType.Adventure,
                NpcId = (int)_npcId,
                Level = _adventureMenu.SelectedLevel,
            };
            ClientSend.SendTCPData(message, Client.OverworldServer);
        }

        public void ShowMatchmakingCounter()
        {
            _lookingForMatchView.SetActive(true);
            if (_lookingForMatchAnimation != null) return;
            _lookingForMatchAnimation = LookingForMatchTextAnimation();
            StartCoroutine(_lookingForMatchAnimation);
            GameManager.Instance.Player.Enabled = false;
            CameraController.Instance.DisableCameraMovement = true;
        }

        private IEnumerator LookingForMatchTextAnimation()
        {
            int numberOfDots = 0;
            string text;
            while (true)
            {
                text = _BASE_LOOKING_FOR_MATCH_TEXT;
                numberOfDots++;                
                for (int i = 0; i < numberOfDots; i++) text += ".";
                _lookingForMatchText.text = text;
                if (numberOfDots % 3 == 0) numberOfDots = 0;
                yield return new WaitForSeconds(0.75f);
            }
        }

        public void HideMatchmakingView()
        {
            StopCoroutine(_lookingForMatchAnimation);
            _lookingForMatchAnimation = null;
            _lookingForMatchView.SetActive(false);
            GameManager.Instance.Player.Enabled = true;
            CameraController.Instance.DisableCameraMovement = false;
        }

        private void CancelMatchmaking()
        {
            CancelMatchmakingRequestMessage message = new CancelMatchmakingRequestMessage();
            ClientSend.SendTCPData(message, Client.OverworldServer);
        }

        public void LoadInstanceScene()
        {
            SceneManager.LoadScene("Instance");
        }

        public void MatchMakingUnsucessful()
        {
            _selectAdventureLevelButton.interactable = true;
        }

        public void UpdatePvpLeaderboard(LeaderboardData data)
        {
            string leaderboard = "PvP Leaderboard";
            int count = 1;
            data.Leaderboard.ForEach(d =>
            {
                leaderboard = $"{leaderboard}\n#{count} {d.PlayerName} {d.Points}";
                count++;
            });
            _pvpLeaderBoard.text = leaderboard;
        }

        public void UpdateAdventureLeaderboard(LeaderboardData data)
        {
            string leaderboard = "Adventure Leaderboard";
            int count = 1;
            data.Leaderboard.ForEach(d =>
            {
                leaderboard = $"{leaderboard}\n#{count} {d.PlayerName} {d.Points}";
                count++;
            });
            _adventureLeaderBoard.text = leaderboard;
        }

        public void ShowProfileData(string name, int adventureLevel, int pvpRating)
        {
            _profileName.text = name;
            _profileAdventureLevel.text = _BASE_ADVENTURE_LEVEL_TEXT + adventureLevel;
            _provilePvpRating.text = _BASE_PVP_RATING_TEXT + pvpRating;
        }
    }
}
