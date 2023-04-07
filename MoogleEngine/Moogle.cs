namespace MoogleEngine;

public static class Moogle
{
    public static SearchResult Query(string query)
    {
        Vector queryTF_IDF = TF_IDF.ComputeQueryTF_IDF(query);

        SearchItem[] items = new SearchItem[3]
        {
            new SearchItem("Hello World", "Lorem ipsum dolor sit amet", 0.9f),
            new SearchItem("Hello World", "Lorem ipsum dolor sit amet", 0.5f),
            new SearchItem("Hello World", "Lorem ipsum dolor sit amet", 0.1f),
        };

        return new SearchResult(items, query);
    }

    public static void Testing()
    {
        return;
    }
}
