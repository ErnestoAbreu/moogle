namespace MoogleEngine;

public class DocumentReader
{
    public static string[] GetWords(string text)
    {
        char[] separator = { ' ', ',', '.', ':', ';', '\t', '\n' };

        string[] words = text.Split(separator);
        List<string> wordsList = new List<string>();

        for (int i = 0; i < words.Length; i++)
        {
            if (Normalize(words[i]) != "")
                wordsList.Add(Normalize(words[i]));
        }

        words = new string[wordsList.Count];
        for (int i = 0; i < wordsList.Count; i++)
            words[i] = wordsList[i];

        return words;
    }

    private static string Normalize(string word)
    {
        string newWord = "";
        for (int i = 0; i < word.Length; i++)
        {
            if (IsLetter(word[i]))
                newWord += word[i];
        }

        return newWord;
    }

    private static bool IsLetter(char c)
    {
        return (c >= 'a' && c <= 'z')
            || (c >= 'A' && c <= 'Z')
            || c == 'á'
            || c == 'é'
            || c == 'í'
            || c == 'ó'
            || c == 'ú'
            || c == 'ñ'
            || c == 'Á'
            || c == 'Á'
            || c == 'Í'
            || c == 'Ó'
            || c == 'Ú'
            || c == 'Ñ'
            || c == '0'
            || c == '1'
            || c == '2'
            || c == '3'
            || c == '4'
            || c == '5'
            || c == '6'
            || c == '7'
            || c == '8'
            || c == '9';
    }

    public static string[] WordsList(string[] documents)
    {
        Dictionary<string, bool> Word = new Dictionary<string, bool>();

        foreach (string name in documents)
        {
            string[] words = GetWords(File.ReadAllText(name));

            foreach (string word in words)
                Word[word] = true;
        }

        string[] wordsList = new string[Word.Count];
        int k = 0;
        foreach (string word in Word.Keys)
            wordsList[k++] = word;

        return wordsList;
    }

    public static string[] DocumentsList(string path)
    {
        return Directory.GetFiles(path, "*.txt");
    }
}
