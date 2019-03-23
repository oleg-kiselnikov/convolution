namespace MatrixOperations
{
    static class MatrixExtensions
    {
        public static double Sum(this double[,] arr)
        {
            int y = arr.GetLength(1);
            int x = arr.GetLength(0);
            double sum = 0;
            for (int i = 0; i < y; i++)
                for (int j = 0; j < x; j++)
                    sum += arr[i, j];
            return sum;
        }

        public static double Max(this double[,] arr)
        {
            int y = arr.GetLength(1);
            int x = arr.GetLength(0);
            double max = -1;
            for (int i = 0; i < y; i++)
                for (int j = 0; j < x; j++)
                    if (max < arr[i, j])
                        max = arr[i, j];
            return max;
        }
    }
}
