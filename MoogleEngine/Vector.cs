namespace MoogleEngine;

public class Vector : Matrix
{
    public int Dimensions;

    public Vector(int N)
        : base(1, N)
    {
        this.Dimensions = N;
    }

    public float this[int i]
    {
        get { return matrix[0, i]; }
        set { matrix[0, i] = value; }
    }

    /* Producto escalar entre dos vectores */
    static public float Dot_Product(Vector a, Vector b)
    {
        if (a.Dimensions != b.Dimensions)
            return 0;

        float dot_product = 0;
        for (int i = 0; i < a.Dimensions; ++i)
            dot_product += a[i] * b[i];

        return dot_product;
    }

    /* Modulo de un vector */
    public float Module()
    {
        float module = 0;
        for (int i = 0; i < this.Dimensions; i++)
        {
            module += this[i] * this[i];
        }
        return (float)Math.Sqrt(module);
    }
}
