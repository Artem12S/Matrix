using static System.Random;

Console.WriteLine("Введите размерность матрицы: ");
var nums = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
int n = nums[0], k = nums[1];

var m = new Matrix(n, k);
m.Show();
Console.WriteLine(m.Det);
Matrix.Show(m.Matrix_T);

Matrix.Show(m.Inv_matrix);
//var m = new Matrix(3,3);
//int[,] ma = {{1, 0, 0}, {0, 1, 0}, {0, 0 ,1}};
//Console.WriteLine(Matrix.Det_calc(ma));

public class Matrix
{
    public int N { get; init; }
    public int K { get; init; }
    private double? _det;
    public double Det => _det ??= Det_calc(this.matrix);
    private double[,]? _matrix_T;
    public double[,] Matrix_T => _matrix_T ??= Transposition(matrix);

    private double[,]? _inv_matrix;
    public double[,] Inv_matrix => _inv_matrix ??= Inverse(matrix);

    private double[,] matrix;
    public Matrix(int n, int k)
    {
        N = n;
        K = k;
        matrix = new double[N, K];
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

    public static void Show(double[,] m)
    {
        for (int i = 0; i < m.GetLength(0) ; i++)
        {
            for (int j = 0; j < m.GetLength(1); j++)
            {
                Console.Write(m[i, j].ToString("0.##") + " ");
            }
            Console.WriteLine();
        }
    }

    private static double Det_calc(double[,] m)
    {
        if (m.GetLength(0) != m.GetLength(1))
            throw new ArgumentException("Матрица должна быть квадратной");
        
        if (m.Length == 1)
            return m[0, 0];
            
        double s = 0;              
        for (int i = 0; i < m.GetLength(0) ; i++)
        {   
            s += m[i, 0] * Cofactor(m, i, 0);          
        }
        return s;
    }

    private static double[,] M_less(double[,] m, int i, int j)
    {
        int n = m.GetLength(0);
        int k = m.GetLength(1);
        double[,] res = new double[n - 1, k - 1];

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

    private static double Cofactor(double[,] m, int i, int j)
    {
        int sing = (i + j) % 2 == 0 ? 1: -1;
        return sing * Det_calc(M_less(m, i, j));
    }

    public static double[,] Transposition(double[,] m)
    {
        int n = m.GetLength(0);
        int k = m.GetLength(1);
        var res = new double[n, k];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < k; j++)
            {
                res[j, i] = m[i, j];
            }
        }
        return res;
    }

    private double[,] Inverse(double[,] m)
    {
        if (Det - 1e-9 == 0)
            throw new InvalidOperationException("Определитель равен нулю");

        int n = m.GetLength(0);
        int k = m.GetLength(1);
        double[,] M = new double[n, k];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < k; j++)
            {
                M[i, j] = Cofactor(m, i, j) / Det;
            }
        }
        return Transposition(M);
    }
}


