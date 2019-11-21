using MathNet.Numerics.LinearAlgebra;

namespace Somnium.Core.Double
{
    public class LayerPooling : Layer<double>
    {
        public LayerPooling(Matrix<double> datas) : base(datas)
        {
        }
    }
}
