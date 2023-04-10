namespace MoogleEngine;

public class StringUtil
{
    public static double EditDistance(string a, string b)
    {
        int[,] dp = new int[a.Length + 1, b.Length + 1];
        for (int i = 0; i <= a.Length; i++)
            for (int j = 0; j <= b.Length; j++)
            {
                if (i == 0 || j == 0) { 
                    dp[i,j] = Math.Max(i , j);
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
        return (double)dp[a.Length, b.Length];
    }

    public static double LongestCommonPrefix(string a, string b)
    {
        for (int i = 0; i < Math.Min(a.Length, b.Length); i++)
            if (a[i] != b[i])
                return (double)(i + 1);

        return (double)Math.Min(a.Length, b.Length) + 1;
    }

    public static double Distance(string a, string b)
    {
        return EditDistance(a, b) / LongestCommonPrefix(a, b);
    }

    public static string GetSuggetion(string query)
    {
        string suggestion = "";
        bool suggestionNecessary = false;

        /* Comparamos cada palabra de la query con todas las palabras del conjunto de documentos */
        foreach (string word in DocumentReader.GetWords(query))
        {
            string realWord = word;
            double minDistance = 1000;
            foreach (string otherWord in TF_IDF.words)
            {
                double distance = Distance(word, otherWord);
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

        if (!suggestionNecessary)
            suggestion = "";

        return suggestion;
    }
}
