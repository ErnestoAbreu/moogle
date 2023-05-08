namespace MoogleEngine;
using System.Diagnostics;

public static class Moogle
{
    public static SearchResult Query(string query)
    {
        Stopwatch watch = Stopwatch.StartNew();

        Console.WriteLine("\n==== Iniciando una nueva busqueda ====\n");
        Console.WriteLine("Consulta: " + query);

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

        counter = Math.Min(counter, 30);

        /* Escogiendo el resultado de la busqueda */
        Console.WriteLine("Resultados: ");

        SearchItem[] items = new SearchItem[Math.Max(1, counter)];
        if (counter == 0)
        {
            items[0] = new SearchItem("No se encontró resultados para esta búsqueda.", "", 0);
        }
        else
        {
            int k = 0;
            for (int i = scoreList.Length - 1; i >= scoreList.Length - counter; i--)
            {
                string title = "";
                for (int j = 11; j < TF_IDF.documentsName[scoreList[i].index].Length - 4; j++)
                    title += TF_IDF.documentsName[scoreList[i].index][j];

                /* Obteniendo el snippet */
                string snippet = StringUtil.GetSnippet(query, TF_IDF.documentsName[scoreList[i].index]);

                items[k++] = new SearchItem(title, snippet, (float)scoreList[i].score);

                Console.WriteLine("{0}  " + title, (float)scoreList[i].score);
            }
        }

        Console.WriteLine("Busqueda realizada en: {0}s.\n", watch.ElapsedMilliseconds / 1000);

        return new SearchResult(items, suggestion);
    }

    public static void Testing()
    {
        /* Esta método no cumple ningun propósito esencial en el funcionamiento de la aplicación */
    }
}
