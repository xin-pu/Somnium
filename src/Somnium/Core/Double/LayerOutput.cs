using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Somnium.Core.Double
{
    public class LayerOutput : Layer<double>
    {
        public NerveCellActivate NerveCell { set; get; }

        public LayerOutput(Matrix<double> datas, double outputData) : base(datas)
        {
            DatasInput = DenseMatrix.Create(1, 1, outputData);
        }
    }
}
