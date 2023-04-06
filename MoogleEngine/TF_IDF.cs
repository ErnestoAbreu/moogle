namespace MoogleEngine;

public class TF_IDF
{
    public static Matrix tf_idf = new Matrix(0, 0);

    public static void Compute()
    {
        string[] documents = DocumentReader.DocumentsList("..\\Content");
        int numberOfDocuments = documents.Length;
        Dictionary<string, int> Document = new Dictionary<string, int>();

        for (int i = 0; i < numberOfDocuments; i++)
        {
            Document[documents[i]] = i;
        }

        string[] words = DocumentReader.WordsList(documents);
        int numberOfWords = words.Length;
        Dictionary<string, int> Word = new Dictionary<string, int>();
        for (int i = 0; i < numberOfWords; i++)
        {
            Word[words[i]] = i;
        }

        Matrix TF = ComputeTF(Document, Word);
        Vector IDF = ComputeIDF(Document, Word);

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
            string[] text = DocumentReader.GetWords(documentName);

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
            string[] text = DocumentReader.GetWords(documentName);

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
