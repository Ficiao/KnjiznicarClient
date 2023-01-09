using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MatchInstance
{
    public class LetterController : MonoBehaviour
    {
        [SerializeField] private Transform _letterContainer;
        [SerializeField] private WordContainer _wordContainer;
        [SerializeField] private Button _letterButton;
        [SerializeField] private TextMeshProUGUI _letterText;
        private bool _isInWord;
        private char _letter;

        public char Letter
        {
            get => _letter;
            set
            {
                _letter = value;
                _letterText.text = value.ToString();
            }
        }

        public (int, int) Index;

        private void Start()
        {
            _letterButton.onClick.AddListener(ButtonClick);
        }

        public void ResetPoisition()
        {
            if (_isInWord)
            {
                _wordContainer.RemoveLetter(this);
                _isInWord = false;
                transform.parent = _letterContainer;
                transform.localPosition = Vector3.zero;
            }            
        }

        private void ButtonClick()
        {
            if (_isInWord)
            {
                _isInWord = false;
                _wordContainer.RemoveLetter(this);
                transform.parent = _letterContainer;
                transform.localPosition = Vector3.zero;
            }
            else
            {
                _isInWord = true;
                _wordContainer.AddLetter(this);
                transform.parent = _wordContainer.transform;
            }
        }
    }
}