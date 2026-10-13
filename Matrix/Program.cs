using static System.Random;

Console.WriteLine("Введите размерность матрицы: ");
var nums = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
int n = nums[0], k = nums[1];

var m = new Matrix(n, k);
m.Show();
Console.WriteLine(m.Det);
Matrix.Show(m.Matrix_T);

//var m = new Matrix(3,3);
//int[,] ma = {{1, 0, 0}, {0, 1, 0}, {0, 0 ,1}};
//Console.WriteLine(Matrix.Det_calc(ma));



public class Matrix
{
    public int N { get; init; }
    public int K { get; init; }
    private int? _det;
    public int Det => _det ??= Det_calc(this.matrix);
    private int[,]? _matrix_T;
    public int[,] Matrix_T => _matrix_T ??= Transposition(matrix);

    private int[,]? _inv_matrix;
    public int[,] Inv_matrix => _inv_matrix ??= Inverse(matrix);

    private int[,] matrix;
    public Matrix(int n, int k)
    {
        N = n;
        K = k;
        matrix = new int[N, K];
        var R = new Random();

        for (int i = 0; i < N ; i++)
        {
            for (int j = 0; j < K; j++)
            {
                matrix[i, j] = R.Next(1, 10);
            }
        }
    }

    public void Show() => Show(matrix);

    public static void Show(int[,] m)
    {
        for (int i = 0; i < m.GetLength(0) ; i++)
        {
            for (int j = 0; j < m.GetLength(1); j++)
            {
                Console.Write(m[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    private static int Det_calc(int[,] m)
    {
        if (m.GetLength(0) != m.GetLength(1))
            throw new ArgumentException("Матрица должна быть квадратной");
        
        if (m.Length == 1)
            return m[0, 0];
            
        int s = 0;              
        for (int i = 0; i < m.GetLength(0) ; i++)
        {   
            s += m[i, 0] * Cofactor(m, i, 0);          
        }
        return s;
    }

    private static int[,] M_less(int[,] m, int i, int j)
    {
        int n = m.GetLength(0);
        int k = m.GetLength(1);
        int[,] res = new int[n - 1, k - 1];

        for (int r = 0, rr = 0; r < n; r++)
        {
            if (r == i) continue;

            for (int c = 0, cc = 0; c < k; c++)
            {
                if (c == j) continue;
                res[rr, cc] = m[r, c];
                cc++;
            }
            rr++;
        }
        return res;
    }

    private static int Cofactor(int[,] m, int i, int j)
    {
        int sing = i + j % 2 == 0 ? 1: -1;
        return sing * Det_calc(M_less(m, i, 0));
    }

    public static int[,] Transposition(int[,] m)
    {
        int n = m.GetLength(0);
        int k = m.GetLength(1);
        var res = new int[n, k];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < k; j++)
            {
                res[j, i] = m[i, j];
            }
        }
        return res;
    }

    private int[,] Inverse(int[,] m)
    {
        if (Det == 0)
            throw new InvalidOperationException("Определитель равен нулю");

        int n = m.GetLength(0);
        int k = m.GetLength(1);
        int[,] M = new int[n, k];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < k; j++)
            {
                M[i, j] =(int)(1.0/ Det) * Cofactor(m, i, j);
            }
        }
        return Transposition(M);
    }
}


