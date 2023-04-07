namespace MoogleEngine;

public static class Moogle
{
    public static SearchResult Query(string query)
    {
        Vector queryTF_IDF = TF_IDF.ComputeQueryTF_IDF(query);

        (double score, int index)[] score = TF_IDF.VectorialModel(queryTF_IDF);

        Array.Sort(score);

        int counter = 0;
        for (int i = score.Length - 1; i >= 0; --i)
        {
            if (score[i].score != 0)
                counter++;
        }
        SearchItem[] items = new SearchItem[counter];

        int k = 0;
        for (int i = score.Length - 1; i >= score.Length - counter; i--){
            items[k++] = new SearchItem(TF_IDF.documents[score[i].index], "Lorem ipsum dolor sit amet", (float)score[i].score);
        }

        return new SearchResult(items, query);
    }

    public static void Testing()
    {
        return;
    }
}
