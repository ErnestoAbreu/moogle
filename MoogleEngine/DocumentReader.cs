namespace MoogleEngine;

public class DocumentReader
{
    public static string[] GetWords(string text)
    {
        /* Devuelve un arreglo con las palabras del texto */
        char[] separator = { ' ', ',', '.', ':', ';', '\t', '\n' };
        string[] words = text.Split(separator, StringSplitOptions.RemoveEmptyEntries);

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
        /* Le quita los caracteres extraños a la palabra */
        string newWord = "";
        for (int i = 0; i < word.Length; i++)
        {
            if (IsLetter(word[i]))
                newWord += word[i];
        }

        return newWord.ToLower();
    }

    private static bool IsLetter(char c)
    {
        /* Devuelve true si el caracter es valido en una palabra */
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

    public static string[] WordsList(string[] documentsName)
    {
        /* Devuelve un arreglo con las palabras de los documentos */

        Dictionary<string, bool> Word = new Dictionary<string, bool>();

        foreach (string name in documentsName)
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

    public static string[] DocumentsNameList(string path)
    {
        /* Devuelve un arreglo con los nombres de los documentos de una direccion */

        return Directory.GetFiles(path, "*.txt");
    }
}
