namespace MoogleEngine;

public static class Moogle
{
    public static SearchResult Query(string query)
    {
        Debug.StartTime();
        Debug.Write("==== Iniciando una nueva busqueda====");
        Debug.Write(query);

        /* Obteneniendo la sugerencia */
        string suggestion = StringUtil.GetSuggetion(query);

        /* Obteniendo las puntuaciones de cada documento*/
        Vector queryTF_IDF = TF_IDF.ComputeQueryTF_IDF(query);
        (float score, int index)[] scoreList = TF_IDF.VectorialModel(queryTF_IDF);

        /* Ordenandolos por puntuacion */
        Array.Sort(scoreList);

        /* Quitando los que no tienen relevancia ninguna */
        int counter = 0;
        for (int i = scoreList.Length - 1; i >= 0; --i)
            if (scoreList[i].score > (1e-9))
                counter++;

        /* Escogiendo el resultado de la busqueda */
        Debug.Write("Resultado: ");
        SearchItem[] items = new SearchItem[counter];

        int k = 0;
        for (int i = scoreList.Length - 1; i >= scoreList.Length - counter; i--)
        {
            string title = "";
            for (int j = 11; j < TF_IDF.documents[scoreList[i].index].Length - 4; j++)
                title += TF_IDF.documents[scoreList[i].index][j];

            /* Obteniendo el snippet */
            // string snippet = File.ReadAllText(TF_IDF.documents[scoreList[i].index]);

            items[k++] = new SearchItem(
                title,
                "Lorem ipsum dolor sit amet",
                (float)scoreList[i].score
            );
            Debug.Write("{0}  " + title, (float)scoreList[i].score);
        }

        Debug.Write("Busqueda realizada en: {0}s", Debug.GetTime());

        return new SearchResult(items, suggestion);
    }

    public static void Testing()
    {
        // int index = TF_IDF.Word["harry"];
        // Console.Write(TF_IDF.idf[index] + "  ");
        // Console.WriteLine(index);

        // foreach (string title in TF_IDF.documents)
        // {
        //     Console.Write(title + ": ");
        //     Console.Write(TF_IDF.tf[index, TF_IDF.Document[title]] + "  ");
        //     Console.WriteLine(TF_IDF.tf_idf[index, TF_IDF.Document[title]]);
        // }
    }
}
