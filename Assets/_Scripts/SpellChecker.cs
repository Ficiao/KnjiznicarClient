using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class SpellChecker
{   
    public static bool Validate(string text)
    {
        return WordLibrary.Instance.WordExists(text);
    }
}
