using MathNet.Numerics.LinearAlgebra;

namespace Somnium.Core.Double
{
    public class LayerInput : Layer<double>
    {
        public double ExceptVal { set; get; }

        public LayerInput(Matrix<double> datas, double exceptVal) : base(datas)
        {
            ExceptVal = exceptVal;
        }
    }
}
