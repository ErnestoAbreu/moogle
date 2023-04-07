namespace MoogleEngine;

public class Matrix
{
    public int rows;
    public int columns;
    public double[,] matrix;

    // Constructor de la matriz
    public Matrix(int rows, int columns)
    {
        this.rows = rows;
        this.columns = columns;
        this.matrix = new double[rows, columns];
    }

    // Definicion del operador indizador
    public double this[int i, int j]
    {
        get { return matrix[i, j]; }
        set { matrix[i, j] = value; }
    }

    // Definicion de multiplicacion de matrices
    public static Matrix operator *(Matrix A, Matrix B)
    {
        if (A.columns != B.rows)
        {
            // Exception
            return new Matrix(0, 0);
        }
        else
        {
            Matrix C = new Matrix(A.rows, B.columns);
            for (int i = 0; i < C.rows; ++i)
                for (int j = 0; j < C.columns; ++j)
                    for (int k = 0; k < A.columns; ++k)
                    {
                        C[i, j] += A[i, k] * B[k, j];
                    }
            return C;
        }
    }

    // Definicion de multiplicacion de un escalar por una matriz
    public static Matrix operator *(double x, Matrix A)
    {
        Matrix C = new Matrix(A.rows, A.columns);

        for (int i = 0; i < A.rows; ++i)
            for (int j = 0; j < A.columns; ++j)
                C[i, j] = A[i, j] * x;

        return C;
    }

    // Definicion de matriz transpuesta
    public void Transpose()
    {
        Matrix newMatrix = new Matrix(this.columns, this.rows);

        for (int i = 0; i < this.rows; ++i)
            for (int j = 0; j < this.columns; ++j)
                newMatrix[j, i] = this[i, j];
    }

    // Imprime la matriz
    public void debug()
    {
        for (int i = 0; i < this.rows; ++i)
        {
            for (int j = 0; j < this.columns; ++j)
            {
                Console.Write(this[i, j]);
                Console.Write(' ');
            }
            Console.Write('\n');
        }
    }
}
