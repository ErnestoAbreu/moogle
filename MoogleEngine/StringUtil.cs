namespace MoogleEngine;

public class StringUtil
{
    public static float EditDistance(string a, string b)
    {
        /* Calcula el EditDistance para dos cadenas de caracteres */

        int[,] dp = new int[a.Length + 1, b.Length + 1];
        for (int i = 0; i <= a.Length; i++)
            for (int j = 0; j <= b.Length; j++)
            {
                if (i == 0 || j == 0)
                {
                    dp[i, j] = Math.Max(i, j);
                }
                else
                {
                    dp[i, j] = Int32.MaxValue;
                    if (a[i - 1] == b[j - 1])
                        dp[i, j] = dp[i - 1, j - 1];
                    else
                    {
                        dp[i, j] = Math.Min(dp[i, j], dp[i - 1, j] + 1);
                        dp[i, j] = Math.Min(dp[i, j], dp[i, j - 1] + 1);
                        dp[i, j] = Math.Min(dp[i, j], dp[i - 1, j - 1] + 1);
                    }
                }
            }
        return (float)dp[a.Length, b.Length];
    }

    public static float LongestCommonPrefix(string a, string b)
    {
        /* Calcula el LongestCommonPrefix para dos cadenas de caracteres */

        for (int i = 0; i < Math.Min(a.Length, b.Length); i++)
            if (a[i] != b[i])
                return (float)(i + 1);

        return (float)Math.Min(a.Length, b.Length) + 1;
    }

    public static float Distance(string a, string b)
    {
        /* Calcula la distancia entre dos cadenas de caracteres */

        return EditDistance(a, b) / LongestCommonPrefix(a, b);
    }

    public static string GetSuggetion(string query)
    {
        /* Devuelve la sugerencia para una consulta */

        string suggestion = "";
        bool suggestionNecessary = false;

        /* Comparamos cada palabra de la query con todas las palabras del conjunto de documentos */
        foreach (string word in DocumentReader.GetWords(query))
        {
            string realWord = word;
            float minDistance = 1000;
            foreach (string otherWord in TF_IDF.words)
            {
                float distance = Distance(word, otherWord);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    realWord = otherWord;
                }
            }
            suggestion += realWord;
            suggestion += " ";

            if (word != realWord)
                suggestionNecessary = true;
        }

        /* Si la sugerencia es igual a la consulta no la tenemos en cuenta */
        if (!suggestionNecessary)
            suggestion = "";

        return suggestion;
    }

    public static string GetSnippet(string query, string title)
    {
        /* Devuelve el snippet de un documento para una consulta */
        
        string snippet = "";

        Vector queryTF_IDF = TF_IDF.ComputeQueryTF_IDF(query);

        string[] words = DocumentReader.GetWords(File.ReadAllText(title));

        int left = 0,
            right = 0;
        float score = 0,
            bestScore = 0;
        for (int l = 0, r = 0; r < words.Length; r++)
        {
            if (r - l > 100)
            {
                if (TF_IDF.Word.ContainsKey(words[l]))
                    score -=
                        queryTF_IDF[TF_IDF.Word[words[l]]]
                        * TF_IDF.tf_idf[TF_IDF.Word[words[l]], TF_IDF.Document[title]];
                l++;
            }
            if (TF_IDF.Word.ContainsKey(words[r]))
                score +=
                    queryTF_IDF[TF_IDF.Word[words[r]]]
                    * TF_IDF.tf_idf[TF_IDF.Word[words[r]], TF_IDF.Document[title]];

            /* Escogemos la subsecuencia con mayor relevancia con la consulta */
            if (score >= bestScore)
            {
                left = l;
                right = r;
                bestScore = score;
            }
        }

        /* Marcamos las palabras que aparecen en la consulta */
        Dictionary<string, bool> Mark = new Dictionary<string, bool>();
        string[] queryWords = DocumentReader.GetWords(query);
        for (int i = 0; i < queryWords.Length; ++i)
            Mark[queryWords[i]] = true;

        /* Escogemos la subsecuencia de palabras para el snippet */
        for (int i = left; i <= right; ++i)
        {
            /* Si la palabra aparece en la consulta le ponemos en negrita */
            if (Mark.ContainsKey(words[i]))
            {
                // snippet += "u001b[1m";
                snippet += words[i];
                // snippet += "u001b[0m";
            }
            else
                snippet += words[i];

            snippet += ' ';
        }
        return snippet;
    }
}
