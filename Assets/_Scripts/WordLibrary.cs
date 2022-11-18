using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class WordLibrary : Singleton<WordLibrary>
{
    [SerializeField] private string _dictionaryPath = null;
    private Dictionary<char, RecursiveDictionary> _rjecnik = null;

    protected override void Init()
    {
        _rjecnik = new Dictionary<char, RecursiveDictionary>();
        string kek = File.ReadAllText(_dictionaryPath);
        List<string> rijeci = kek.Split("\n".ToCharArray()).ToList();
        RecursiveDictionary recursive;
        foreach(string rijec in rijeci)
        {
            if (rijec.Length < 1) continue;
            if (_rjecnik.ContainsKey(rijec[0]))
            {
                recursive = _rjecnik[rijec[0]];
            }
            else
            {
                recursive = new RecursiveDictionary();
                _rjecnik.Add(rijec[0], recursive);
            }
          
            for(int i = 1; i < rijec.Length; i++)
            {
                recursive = recursive.CreateShelve(rijec[i]);
            }
        }
    }

    public bool WordExists(string word)
    {
        if (_rjecnik.ContainsKey(word[0]) == false) return false;
        RecursiveDictionary recursive = _rjecnik[word[0]];

        for (int i = 1; i < word.Length; i++)
        {
            if (recursive.Contains(word[i]) == false) return false;
            recursive = recursive.GetShelve(word[i]);
        }

        return true;
    }
}
