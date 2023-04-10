namespace MoogleEngine;

public class TF_IDF
{
    public static Matrix tf_idf = new Matrix(0, 0);
    public static Vector idf = new Vector(0);
    public static string[] documents = { };
    public static string[] words = {};
    public static Dictionary<string, int> Document = new Dictionary<string, int>();
    public static Dictionary<string, int> Word = new Dictionary<string, int>();

    public static (double, int)[] VectorialModel(Vector queryTF_IDF)
    {
        (double, int)[] vectorialModel = new (double, int)[tf_idf.columns];

        for (int j = 0; j < tf_idf.columns; j++)
        {
            Vector v = new Vector(tf_idf.rows);
            for (int i = 0; i < tf_idf.rows; i++)
            {
                v[i] = tf_idf[i, j];
            }
            double score = 0;

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

        double maxTF = 1;
        foreach (string word in text)
        {
            if (Word.ContainsKey(word))
            {
                queryTF_IDF[Word[word]]++;
                maxTF = Math.Max(maxTF, queryTF_IDF[Word[word]]);
            }
        }

        foreach (string word in text)
        {
            if (Word.ContainsKey(word))
            {
                queryTF_IDF[Word[word]] /= maxTF;
                queryTF_IDF[Word[word]] *= idf[Word[word]];
            }
        }

        return queryTF_IDF;
    }

    public static void Compute()
    {
        documents = DocumentReader.DocumentsList("..\\Content");
        int numberOfDocuments = documents.Length;

        for (int i = 0; i < numberOfDocuments; i++)
        {
            Document[documents[i]] = i;
        }

        words = DocumentReader.WordsList(documents);
        int numberOfWords = words.Length;

        for (int i = 0; i < numberOfWords; i++)
        {
            Word[words[i]] = i;
        }

        Matrix TF = ComputeTF(Document, Word);
        Vector IDF = ComputeIDF(Document, Word);
        idf = IDF;

        Matrix TF_IDF = new Matrix(numberOfWords, numberOfDocuments);

        for (int j = 0; j < numberOfDocuments; j++)
            for (int i = 0; i < numberOfWords; i++)
                TF_IDF[i, j] = TF[i, j] * IDF[i];

        tf_idf = TF_IDF;
    }

    public static Matrix ComputeTF(Dictionary<string, int> Document, Dictionary<string, int> Word)
    {
        int numberOfDocuments = Document.Count;
        int numberOfWords = Word.Count;

        Matrix TF = new Matrix(numberOfWords, numberOfDocuments);

        foreach (string documentName in Document.Keys)
        {
            string[] text = DocumentReader.GetWords(File.ReadAllText(documentName));

            double maxTF = 1;
            foreach (string word in text)
            {
                TF[Word[word], Document[documentName]]++;
                maxTF = Math.Max(maxTF, TF[Word[word], Document[documentName]]);
            }
            // Console.WriteLine(maxTF);

            foreach (string word in text)
            {
                TF[Word[word], Document[documentName]] /= maxTF;
            }
        }
        return TF;
    }

    public static Vector ComputeIDF(Dictionary<string, int> Document, Dictionary<string, int> Word)
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
                IDF[i] = Math.Log10(numberOfDocuments / (IDF[i]));
        }

        return IDF;
    }
}
