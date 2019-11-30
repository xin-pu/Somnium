using MathNet.Numerics.LinearAlgebra.Double;

namespace Somnium.Kernel
{
    public class DataFlow
    {
        public Matrix[] InputData { set; get; }
        public double[] ExpectedOut { set; get; }
        public double[] ActualOut { set; get; }
        public string ExpectedLabel { set; get; }
        public string ActualLabel { set; get; }

    }
}
