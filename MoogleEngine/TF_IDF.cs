namespace MoogleEngine;
using System.Diagnostics;

public class TF_IDF
{
    public static Matrix tf_idf = new Matrix(0, 0);
    public static Matrix tf = new Matrix(0, 0);
    public static Vector idf = new Vector(0);
    public static string[] documentsName = { };
    public static string[] words = { };
    public static Dictionary<string, int> Document = new Dictionary<string, int>();
    public static Dictionary<string, int> Word = new Dictionary<string, int>();

    public static (float, int)[] VectorialModel(Vector queryTF_IDF)
    {
        (float, int)[] vectorialModel = new (float, int)[tf_idf.columns];

        for (int j = 0; j < tf_idf.columns; j++)
        {
            Vector v = new Vector(tf_idf.rows);
            for (int i = 0; i < tf_idf.rows; i++)
            {
                v[i] = tf_idf[i, j];
            }
            float score = 0;

            if (queryTF_IDF.Module() * v.Module() != 0)
                score = Vector.Dot_Product(queryTF_IDF, v) / (queryTF_IDF.Module() * v.Module());

            vectorialModel[j] = (score, j);
        }

        return vectorialModel;
    }

    public static Vector ComputeQueryTF_IDF(string query)
    {
        Vector queryTF_IDF = new Vector(tf_idf.rows);

        string[] text = DocumentReader.GetWords(query);

        float maxTF = 1;
        foreach (string word in text)
        {
            if (Word.ContainsKey(word))
            {
                queryTF_IDF[Word[word]]++;
                maxTF = Math.Max(maxTF, queryTF_IDF[Word[word]]);
            }
        }

        Dictionary<string, bool> Mark = new Dictionary<string, bool>();
        foreach (string word in text)
        {
            if (!Mark.ContainsKey(word))
            {
                queryTF_IDF[Word[word]] /= maxTF;
                queryTF_IDF[Word[word]] *= idf[Word[word]];
                Mark[word] = true;
            }
        }

        return queryTF_IDF;
    }

    public static void Compute()
    {
        /* Precalcula el TF_IDF */

        Stopwatch watch = Stopwatch.StartNew();
        Console.WriteLine("1. Calculando TF_IDF:");

        Console.WriteLine("- Leyendo documentos.");

        /* Arreglo con los nombres de los documentos */
        documentsName = DocumentReader.DocumentsNameList("..\\Content");
        int numberOfDocuments = documentsName.Length;

        /* Diccionario [nombre de documento => indice] */
        for (int i = 0; i < numberOfDocuments; i++)
        {
            Document[documentsName[i]] = i;
        }

        /* Arreglo con las palabras del los documentos */
        words = DocumentReader.WordsList(documentsName);
        int numberOfWords = words.Length;

        /* Diccionario [palabra => indice] */
        for (int i = 0; i < numberOfWords; i++)
        {
            Word[words[i]] = i;
        }

        Console.WriteLine("- Documentos leidos.");

        Console.WriteLine("- Calculando TF.");

        Matrix TF = ComputeTF();
        tf = TF;

        Console.WriteLine("- TF calculado.");

        Console.WriteLine("- Calculando IDF.");

        Vector IDF = ComputeIDF();
        idf = IDF;

        Console.WriteLine("- IDF calculado.");

        Matrix TF_IDF = new Matrix(numberOfWords, numberOfDocuments);

        /* Multiplicar TF por IDF */
        for (int j = 0; j < numberOfDocuments; j++)
            for (int i = 0; i < numberOfWords; i++)
                TF_IDF[i, j] = TF[i, j] * IDF[i];

        tf_idf = TF_IDF;

        Console.WriteLine("TF_IDF calculado en {0}s.\n", watch.ElapsedMilliseconds / 1000);
    }

    public static Matrix ComputeTF()
    {
        /* Calcula el TF */

        int numberOfDocuments = documentsName.Length;
        int numberOfWords = words.Length;

        Matrix TF = new Matrix(numberOfWords, numberOfDocuments);

        foreach (string documentName in Document.Keys)
        {
            string[] text = DocumentReader.GetWords(File.ReadAllText(documentName));

            float maxTF = 1;
            foreach (string word in text)
            {
                TF[Word[word], Document[documentName]]++;
                maxTF = Math.Max(maxTF, TF[Word[word], Document[documentName]]);
            }

            Dictionary<string, bool> Mark = new Dictionary<string, bool>();

            foreach (string word in text)
            {
                if (!Mark.ContainsKey(word))
                {
                    TF[Word[word], Document[documentName]] /= maxTF;
                    Mark[word] = true;
                }
            }
        }

        return TF;
    }

    public static Vector ComputeIDF()
    {
        /* Calcula el TF */

        int numberOfDocuments = documentsName.Length;
        int numberOfWords = words.Length;

        Vector IDF = new Vector(numberOfWords);

        foreach (string documentName in Document.Keys)
        {
            Dictionary<string, bool> Mark = new Dictionary<string, bool>();

            string[] text = DocumentReader.GetWords(File.ReadAllText(documentName));

            foreach (string word in text)
            {
                if (!Mark.ContainsKey(word))
                {
                    Mark[word] = true;
                    IDF[Word[word]]++;
                }
            }
        }

        for (int i = 0; i < numberOfWords; i++)
        {
            if (IDF[i] != 0)
                IDF[i] = (float)Math.Log10(numberOfDocuments / (IDF[i]));

            /* Acotamos el IDF para quitar toda la relevancia a las stopwords */
            if (IDF[i] < 0.7)
                IDF[i] = 0;
        }

        return IDF;
    }
}
