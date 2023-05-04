namespace MoogleEngine;

public class TF_IDF
{
    public static Matrix tf_idf = new Matrix(0, 0);
    public static Matrix tf = new Matrix(0, 0);
    public static Vector idf = new Vector(0);
    public static string[] documents = { };
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

    /* Precalculando el TF_IDF */
    public static void Compute()
    {
        Debug.StartTime();
        Debug.Write("1. Calculando TF_IDF");

        // Lista de documentos
        documents = DocumentReader.DocumentsList("..\\Content");
        int numberOfDocuments = documents.Length;

        // Diccionario [nombre de documento => indice]
        for (int i = 0; i < numberOfDocuments; i++)
        {
            Document[documents[i]] = i;
        }

        // Lista de todas las palabras de los documentos
        words = DocumentReader.WordsList(documents);
        int numberOfWords = words.Length;

        // Diccionario [palabra => indice]
        for (int i = 0; i < numberOfWords; i++)
        {
            Word[words[i]] = i;
        }
        Debug.Write("- Leyendo los documentos.");

        Matrix TF = ComputeTF();
        Debug.Write("- TF calculado.");
        tf = TF;
        Vector IDF = ComputeIDF();
        Debug.Write("- IDF calculado.");
        idf = IDF;

        Matrix TF_IDF = new Matrix(numberOfWords, numberOfDocuments);

        // Multiplicar TF por IDF
        for (int j = 0; j < numberOfDocuments; j++)
            for (int i = 0; i < numberOfWords; i++)
                TF_IDF[i, j] = TF[i, j] * IDF[i];

        tf_idf = TF_IDF;

        Debug.Write("TF_IDF calculado en {0}s.", Debug.GetTime());
        Debug.Write();
    }

    public static Matrix ComputeTF()
    {
        int numberOfDocuments = Document.Count;
        int numberOfWords = Word.Count;

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
        int numberOfDocuments = Document.Count;
        int numberOfWords = Word.Count;

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
            if (IDF[i] == 0)
                IDF[i] = 0;
            else
                IDF[i] = (float)Math.Log10(numberOfDocuments / (IDF[i]));

            if(IDF[i] < 0.7)
                IDF[i] = 0;
        }

        return IDF;
    }
}
