using System.Collections.Generic;

public class RecursiveDictionary
{
    private Dictionary<char, RecursiveDictionary> _rjecnik = null;

    public RecursiveDictionary()
    {
        _rjecnik = new Dictionary<char, RecursiveDictionary>();
    }

    public RecursiveDictionary CreateShelve(char slovo)
    {
        if (_rjecnik.ContainsKey(slovo) == false)
        {
            _rjecnik.Add(slovo, new RecursiveDictionary());
        }

        return _rjecnik[slovo];
    }

    public RecursiveDictionary GetShelve(char slovo)
    {
        return _rjecnik[slovo];
    }

    public bool Contains(char slovo)
    {
        return _rjecnik.ContainsKey(slovo);
    }
}
