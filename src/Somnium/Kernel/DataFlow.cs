using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Somnium.Kernel
{
    public class DataFlow
    {
        public Matrix[] Data { set; get; }
        public Matrix[] DataCache { set; get; }
        public double[] ExpectedOut { set; get; }
        public double[] ActualOut { set; get; }
        public string ExpectedLabel { set; get; }
        public string ActualLabel { set; get; }

    }
}
