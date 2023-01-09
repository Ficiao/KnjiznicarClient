using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MatchInstance
{
    public class WordContainer : MonoBehaviour
    {
        private List<LetterController> _letters;

        public Action<string, List<(int, int)>> WordChanged;

        private void Awake()
        {
            _letters = new List<LetterController>();
        }

        public void AddLetter(LetterController letterController) 
        {
            _letters.Add(letterController);
            WordChanged?.Invoke(new string(_letters.Select(l => l.Letter).ToArray()), _letters.Select(l => l.Index).ToList());
        }

        public void RemoveLetter(LetterController letterController)
        {
            _letters.Remove(letterController);
            WordChanged?.Invoke(new string(_letters.Select(l => l.Letter).ToArray()), _letters.Select(l => l.Index).ToList());
        }

        public void UpdateLetters(List<char> letters)
        {
            for(int i = _letters.Count - 1;i >= 0; i--)
            {
                _letters[i].Letter = letters[i];
                _letters[i].ResetPoisition();
            }

            WordChanged?.Invoke("", new List<(int, int)>());
        }
    }
}