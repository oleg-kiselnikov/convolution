using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatrixOperations;

namespace Convolution
{
    public class Filter
    {
        private double[,] _kernel;
        public double[,] Kernel
        {
            get
            {
                return _kernel ?? (_kernel = new double[,]
                {
                    {1, 1, 1},
                    {1, 1, 1},
                    {1, 1, 1}
                });
            }
            set
            {
                if (value == null)
                    return; //throw new ArgumentNullException();

                var y = value.GetLength(1);
                var x = value.GetLength(0);

                if (x%2 == 0 || y%2 == 0)
                    throw new ArgumentException("Dimensions of a kernel matrix cannot be even!");
                
                if (x < 3 || y < 3)
                    throw new ArgumentException("Dimensions of a kernel matrix cannot be less than 3x3!");

                _kernel = value;

                _factor = null;
            }
        }

        private double? _factor;

        public double Factor
        {
            get
            {
                if (!_factor.HasValue)
                    _factor = 1/_kernel.Sum();

                return _factor.Value;
            }
            set { _factor = value; }
        }

        public double Bias { get; set; }
    }
}
