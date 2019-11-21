using MathNet.Numerics.LinearAlgebra;

namespace Somnium.Core.Double
{
    public class LayerConvoltion : Layer<double>
    {
        public LayerConvoltion(Matrix<double> datas) : base(datas)
        {
        }
    }
}
