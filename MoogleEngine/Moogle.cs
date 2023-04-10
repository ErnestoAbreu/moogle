namespace MoogleEngine;

public static class Moogle
{
    public static SearchResult Query(string query)
    {
        /* Obteneniendo la sugerencia */
        string suggestion = StringUtil.GetSuggetion(query);

        /* Obteniendo las puntuaciones de cada documento*/
        Vector queryTF_IDF = TF_IDF.ComputeQueryTF_IDF(query);
        (double score, int index)[] scoreList = TF_IDF.VectorialModel(queryTF_IDF);

        /* Ordenandolos por puntuacion */
        Array.Sort(scoreList);

        /* Quitando los que no tienen relevancia ninguna */
        int counter = 0;
        for (int i = scoreList.Length - 1; i >= 0; --i)
            if (scoreList[i].score != 0)
                counter++;

        /* Escogiendo el resultado de la busqueda */
        SearchItem[] items = new SearchItem[counter];

        int k = 0;
        for (int i = scoreList.Length - 1; i >= scoreList.Length - counter; i--)
        {
            string title = "";
            for (int j = 11; j < TF_IDF.documents[scoreList[i].index].Length - 4; j++)
                title += TF_IDF.documents[scoreList[i].index][j];

            items[k++] = new SearchItem(
                title,
                "Lorem ipsum dolor sit amet",
                (float)scoreList[i].score
            );
        }

        return new SearchResult(items, suggestion);
    }

    public static void Testing()
    {
        return;
    }
}
