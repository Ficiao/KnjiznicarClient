using TMPro;
using UnityEngine;
using UnityEngine.UI;
using KnjzinicarAspNetLibraries;
using KnjiznicarDataModel.Message;
using Network;
using UnityEngine.SceneManagement;
using KnjiznicarDataModel.Enum;
using System.Collections.Generic;
using System;
using System.Collections;
using Shared;

namespace MatchInstance
{
    class UIManager : Singleton<UIManager>
    {
        private const string _BASE_RATING_CHANGE_TEXT = "Rating change: ";

        [SerializeField] private GameObject _matchUI;
        [SerializeField] private GameObject _endGameScreen;
        [SerializeField] private GameObject _winText;
        [SerializeField] private GameObject _loseText;
        [SerializeField] private GameObject _adventureIncreaseText;
        [SerializeField] private TextMeshProUGUI _ratingChangeText;
        [SerializeField] private Color _winColor;
        [SerializeField] private Color _loseColor;
        [SerializeField] private Button _continueButton;
        [SerializeField] private TextMeshProUGUI _pingText;
        [SerializeField] private Slider _playerHealthSlider;
        [SerializeField] private TextMeshProUGUI _playerHealthText;
        [SerializeField] private Slider _enemyHealthSlider;
        [SerializeField] private TextMeshProUGUI _enemyHealthText;
        [SerializeField] private GameObject _waitingForOpponentText;
        [SerializeField] private Button _selectWordButton;
        [SerializeField] private Button _surrenderButton;
        [SerializeField] private WordContainer _wordContainer;
        [SerializeField] private TMP_InputField _chatInput;
        [SerializeField] private TextMeshPro _playerDamageText;
        [SerializeField] private TextMeshPro _enemyDamageText;
        [SerializeField] private int _lowDamageThreshold;
        [SerializeField] private Color _lowDamageColor;
        [SerializeField] private int _mediumDamageThreshold;
        [SerializeField] private Color _mediumDamageColor;
        [SerializeField] private int _highDamageThreshold;
        [SerializeField] private Color _highDamageColor;
        [SerializeField] private GameObject _gridMask;
        [SerializeField] private GameObject _wordContainerMask;
        private string _currentWord;
        private List<(int, int)> _currentIndexes;
        private WordChecker _wordChecker;
        private bool _selectWordDisabled;

        private void Awake()
        {
            base.Awake();
            _selectWordButton.onClick.AddListener(SelectWord);
            _surrenderButton.onClick.AddListener(Surrender);
            _continueButton.onClick.AddListener(ContinueFromInstance);
            _chatInput.onSubmit.AddListener(text => SendChatMessage(text));
            _wordContainer.WordChanged += WordChanged;
            string path = Application.streamingAssetsPath;
            _wordChecker = new WordChecker(path + "/CroatianNew.aff", path + "/CroatianNew.dic");
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
            if (time <= 50) _pingText.color = Color.green;
            else if (time <= 75) _pingText.color = Color.yellow;
            if (time <= 35) _pingText.color = Color.green;
            _pingText.text = $"{time} ms";
        }

        public void SetPlayerHealth(int currentHealth, int maximumHealth)
        {
            _playerHealthSlider.value = (float)currentHealth / maximumHealth;
            _playerHealthText.text = currentHealth.ToString();
        }

        public void SetEnemyHealth(int currentHealth, int maximumHealth)
        {
            _enemyHealthSlider.value = (float)currentHealth / maximumHealth;
            _enemyHealthText.text = currentHealth.ToString();
        }

        public void ShowWaitingForOpponent()
        {
            _waitingForOpponentText.SetActive(true);
        }

        public void HideWaitingForOpponent()
        {
            _waitingForOpponentText.SetActive(false);
        }

        private void Surrender()
        {
            SurrenderRequestMessage message = new SurrenderRequestMessage();
            ClientSend.SendTCPData(message, Client.OverworldServer);
        }

        private void SelectWord()
        {
            SelectedWordMessage message = new SelectedWordMessage()
            {
                LetterIndexes = _currentIndexes,
            };
            ClientSend.SendTCPData(message, Client.OverworldServer);
            _selectWordButton.interactable = false;
            _selectWordDisabled = true;
            _gridMask.SetActive(true);
            _wordContainerMask.SetActive(true);
        }

        public void IllegealWordUsed()
        {
            _selectWordDisabled = false;
            _gridMask.SetActive(false);
            _wordContainerMask.SetActive(false);
            WordChanged(_currentWord, _currentIndexes);
        }

        private void WordChanged(string newWord, List<(int, int)> indexes)
        {
            bool isAllowed = newWord.Length == 1 || _wordChecker.IsRealWord(newWord.ToLower());
            _selectWordButton.interactable = isAllowed && !_selectWordDisabled;
            if (isAllowed)
            {
                _currentWord = newWord.ToLower();
                _currentIndexes = indexes;
            }
        }

        public void ShowMatchEndView(MatchEndMessage message)
        {
            bool adventureIncrease = false;
            switch (message.MatchType)
            {
                case MatchType.Pvp:
                    GlobalPlayerData.PlayerData.PvpPoints += (int)message.PvpRatingChange;
                    break;
                case MatchType.Adventure:
                    if(GlobalPlayerData.PlayerData.AdventureLevel < (int)message.AdventureLevel)
                    {
                        adventureIncrease = true;
                        GlobalPlayerData.PlayerData.AdventureLevel = (int)message.AdventureLevel;
                    }
                    break;
                default:
                    Debug.Log($"Ending match for unsupported match type: {message.MatchType}");
                    break;
            }

            _matchUI.SetActive(false);
            _endGameScreen.SetActive(true);

            if (message.HasWon)
            {
                _winText.SetActive(true);
                if (message.MatchType == MatchType.Adventure && adventureIncrease) 
                    _adventureIncreaseText.SetActive(true);
            }
            else _loseText.SetActive(true);

            
            if(message.MatchType == MatchType.Pvp)
            {
                _ratingChangeText.text = $"{_BASE_RATING_CHANGE_TEXT}{message.PvpRatingChange}";
                if (message.PvpRatingChange < 0) _ratingChangeText.color = _loseColor;
                else _ratingChangeText.color = _winColor;
                _ratingChangeText.gameObject.SetActive(true);
            }
        }

        private void ContinueFromInstance()
        {
            SceneManager.LoadScene("Overworld");
            ReturnFromInstanceMessage message = new ReturnFromInstanceMessage();
            ClientSend.SendTCPData(message, Client.OverworldServer);
        }

        public (AnimationType, AnimationType) SetDamageText(string ownWord, int ownDamage, string enemyWord, int enemyDamage)
        {
            (AnimationType, AnimationType) returnValue;

            if (enemyWord != null) _enemyDamageText.text = enemyWord + " " + enemyDamage;
            else _enemyDamageText.text = enemyDamage.ToString();

            if (enemyDamage < _mediumDamageThreshold)
            {
                _enemyDamageText.color = _lowDamageColor;
                returnValue.Item2 = AnimationType.Short;
            }
            else if (enemyDamage < _highDamageThreshold)
            {
                _enemyDamageText.color = _mediumDamageColor;
                returnValue.Item2 = AnimationType.Medium;
            }
            else
            {
                _enemyDamageText.color = _highDamageColor;
                returnValue.Item2 = AnimationType.Long;
            }

            _playerDamageText.text = ownWord + " " + ownDamage;
            if (ownDamage < _mediumDamageThreshold)
            {
                _playerDamageText.color = _lowDamageColor;
                returnValue.Item1 = AnimationType.Short;
            }
            else if (ownDamage < _highDamageThreshold)
            {
                _playerDamageText.color = _mediumDamageColor;
                returnValue.Item1 = AnimationType.Medium;
            }
            else
            {
                _playerDamageText.color = _highDamageColor;
                returnValue.Item1 = AnimationType.Long;
            }

            return returnValue;
        }

        public void ShowPlayerDamage() => StartCoroutine(DamageShower(_playerDamageText.gameObject));

        public void ShowEnemyDamage() => StartCoroutine(DamageShower(_enemyDamageText.gameObject));

        private IEnumerator DamageShower(GameObject text)
        {
            text.SetActive(true);
            yield return new WaitForSeconds(1);
            text.SetActive(false);
        }

        public void EnableSelection()
        {
            _gridMask.SetActive(false);
            _wordContainerMask.SetActive(false);
            _selectWordDisabled = false;
        }

        public void UpdateLetters(List<char> letters) => _wordContainer.UpdateLetters(letters);
    }
}
