using System;

namespace Convolution
{
    public class FurierFilter : Filter
    {
        public FurierFilter(int size, double sigma)
        {
            Kernel = new double[size, size];

            for (int i = 1; i < size + 1; i++)
                for (int j = 1; j < size + 1; j++)
                    Kernel[i-1, j - 1] = Math.Exp((double)(-i * i * j * j) / (2 * sigma * sigma)) / (2 * Math.PI * sigma * sigma);

            /*var r = (size - 1)/2;

            for (int i = -r; i < r + 1; i++)
                for (int j = -r; j < r + 1; j++)
                    Kernel[i + r, j + r] = Math.Exp((double)(-i * i * j * j) / (2 * size * size)) / (2 * Math.PI * size * size);*/
        }
    }

    public class FurierFilter2 : Filter
    {
        public FurierFilter2(int size, double sigma)
        {
            Kernel = new double[size, size];

            for (int i = 1; i < size + 1; i++)
                for (int j = 1; j < size + 1; j++)
                    Kernel[i - 1, j - 1] = 1 - Math.Exp((double)(-i * i * j * j) / (2 * sigma * sigma)) / (2 * Math.PI * sigma * sigma);

            /*var r = (size - 1)/2;

            for (int i = -r; i < r + 1; i++)
                for (int j = -r; j < r + 1; j++)
                    Kernel[i + r, j + r] = Math.Exp((double)(-i * i * j * j) / (2 * size * size)) / (2 * Math.PI * size * size);*/
        }
    }
}
