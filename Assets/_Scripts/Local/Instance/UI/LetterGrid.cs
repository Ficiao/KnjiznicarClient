using System;
using UnityEngine;

namespace MatchInstance
{
    public class LetterGrid : MonoBehaviour
    {
        [Serializable]
        private class LetterRow
        {
            public LetterController[] Letters;
        }
        [SerializeField] private LetterRow[] _letterRows;

        public void SetupLetters(char[,] letterMatrix)
        {
            ResetGrid();
            for (int i = 0; i < letterMatrix.GetLength(0); i++)
            {
                for(int j= 0; j < letterMatrix.GetLength(1); j++)
                {
                    _letterRows[i].Letters[j].Letter = letterMatrix[i, j];
                    _letterRows[i].Letters[j].Index = (i, j);
                }
            }
        }

        public void ResetGrid()
        {
            foreach (LetterRow row in _letterRows) foreach (LetterController letterController in row.Letters) letterController.ResetPoisition();
        }
    }
}